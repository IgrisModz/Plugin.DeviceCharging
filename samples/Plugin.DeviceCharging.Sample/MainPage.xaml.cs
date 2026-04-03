using System.Text.RegularExpressions;
using Plugin.DeviceCharging.Sample.Services;
using Plugin.DeviceCharging.Sample.ViewModels;

namespace Plugin.DeviceCharging.Sample;

public partial class MainPage : ContentPage
{
	string lastUrl = string.Empty;
	readonly IChargingService chargingService;
	readonly ISettingsService settingsService;
	bool isUpdatingTheme = false;

	public MainPage(IChargingService chargingService, ISettingsService settingsService)
	{
		this.chargingService = chargingService;
		this.settingsService = settingsService;
		InitializeComponent();

		Application.Current!.RequestedThemeChanged += OnAppThemeChanged;
		chargingService.ChargingStateChanged += OnChargingChanged;

		MainWebView.Navigated += async (s, e) =>
		{
			if (MainRefreshView.IsRefreshing)
			{
				MainRefreshView.IsRefreshing = false;
			}

			if (string.IsNullOrEmpty(e.Url))
			{
				return;
			}

			lastUrl = e.Url;
			UpdateKeepScreenOn(lastUrl);

			// Inject script to monitor theme changes from website to maui
			var jsObserver = @"
				var switchBtn = document.querySelector('a.nightmode_switch');
				if (switchBtn) {
					switchBtn.addEventListener('click', function(e) {
						setTimeout(function() {
							var currentTheme = document.documentElement.getAttribute('theme');
							window.location.href = 'maui://themechange?theme=' + currentTheme;
						}, 100);
					});
				}
			";
			await MainWebView.EvaluateJavaScriptAsync(jsObserver);

			// Initialize website theme to match app theme
			var isAppDarkTheme = Application.Current.RequestedTheme == AppTheme.Dark;
			await UpdateWebsiteThemeAsync(isAppDarkTheme);
		};

		BindingContext = new MainViewModel(() =>
		{
			MainWebView.Reload();
		});
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		// 🔑 Systematic recalculation KeepScreenOn
		UpdateKeepScreenOn(lastUrl);
	}

	protected override void OnDisappearing()
	{
		UpdateKeepScreenOn(null);

		base.OnDisappearing();
	}

	protected override bool OnBackButtonPressed()
	{
		if (MainWebView.CanGoBack)
		{
			MainWebView.GoBack();
			return true;
		}
		return base.OnBackButtonPressed();
	}

	/// <summary>
	/// Handles changes to the device's charging state and updates the screen behavior accordingly.
	/// </summary>
	/// <param name="_">The source of the event, typically the object that raised the charging state change.</param>
	/// <param name="isCharging">A value indicating whether the device is currently charging. <see langword="true"/> if charging; otherwise, <see
	/// langword="false"/>.</param>
	void OnChargingChanged(object? _, bool isCharging)
	{
		// You can add additional logic here if needed, such as logging or updating UI elements to reflect the charging state.
		MainThread.BeginInvokeOnMainThread(() => UpdateKeepScreenOn(lastUrl));
	}

	void UpdateKeepScreenOn(string? url)
	{
		DeviceDisplay.KeepScreenOn = ShouldKeepScreenOn(url);
	}

	bool ShouldKeepScreenOn(string? url)
	{
		var modeString = Preferences.Get(nameof(SettingsViewModel.SelectedKeepScreenOnMode), KeepScreenOnMode.Never.ToString());

		var result = Enum.TryParse<KeepScreenOnMode>(modeString, out var mode);
		if (!result || mode == KeepScreenOnMode.Never)
		{
			return false;
		}

		var isLecture = IsLecturePage(url) ?? false;

		return mode switch
		{
			KeepScreenOnMode.Always => true,
			KeepScreenOnMode.ReadingOnly => isLecture,
			KeepScreenOnMode.OnlyWhenCharging => isLecture && chargingService.IsCharging,
			_ => false
		};
	}

	static bool? IsLecturePage(string? url)
	{
		return !string.IsNullOrWhiteSpace(url) && IsChapterPageRegex().IsMatch(url);
	}

	[GeneratedRegex(@"^https:\/\/novelfire\.net\/book\/[^\/]+\/chapter-\d+\/?$", RegexOptions.IgnoreCase, "fr-FR")]
	private static partial Regex IsChapterPageRegex();

	async void OnAppThemeChanged(object? sender, AppThemeChangedEventArgs e)
	{
		UpdateNativeScrollbarTheme(e.RequestedTheme);

		if (isUpdatingTheme)
		{
			return;
		}

		var isDark = e.RequestedTheme == AppTheme.Dark;
		await UpdateWebsiteThemeAsync(isDark);
	}

	async Task UpdateWebsiteThemeAsync(bool isDark)
	{
		isUpdatingTheme = true;

		var theme = isDark ? "dark" : "light";
		var bgcolor = isDark ? "black" : "white";
		var dataContent = isDark ? "Light Theme" : "Dark Theme";
		var iconClass = isDark ? "icon-sun" : "icon-moon";

		var js = $@"
			try {{
				var html = document.documentElement;
				if(html) {{
					html.setAttribute('theme', '{theme}');
					html.setAttribute('bgcolor', '{bgcolor}');
					html.style.colorScheme = '{theme}';
				}}

				var switchBtn = document.querySelector('a.nightmode_switch');
				if (switchBtn) {{
					switchBtn.setAttribute('data-content', '{dataContent}');
					var icon = switchBtn.querySelector('i');
					if (icon) {{
						icon.className = '{iconClass}';
					}}
				}}
			}} catch(e) {{
				console.error(e);
			}}
		";

		await MainWebView.EvaluateJavaScriptAsync(js);
		isUpdatingTheme = false;
	}

	void MainWebView_Navigating(object? sender, WebNavigatingEventArgs e)
	{
		if (e.Url.StartsWith("maui://themechange", StringComparison.OrdinalIgnoreCase))
		{
			e.Cancel = true;

			if (isUpdatingTheme)
			{
				return;
			}

			var isDark = e.Url.Contains("theme=dark", StringComparison.OrdinalIgnoreCase);
			var newTheme = isDark ? AppTheme.Dark : AppTheme.Light;

			isUpdatingTheme = true;

			settingsService.SetTheme(newTheme);
			Application.Current?.UserAppTheme = newTheme;

			UpdateNativeScrollbarTheme(newTheme);

			isUpdatingTheme = false;
		}
	}

	void UpdateNativeScrollbarTheme(AppTheme theme)
	{
#if WINDOWS
		// Force le ScrollView natif sur Windows (WinUI) à actualiser le thème spécifique de sa scrollbar
		if (MainScrollView.Handler?.PlatformView is Microsoft.UI.Xaml.FrameworkElement nativeWindowsView)
		{
			nativeWindowsView.RequestedTheme = theme == AppTheme.Dark
				? Microsoft.UI.Xaml.ElementTheme.Dark
				: Microsoft.UI.Xaml.ElementTheme.Light;
		}
#endif
	}
}
