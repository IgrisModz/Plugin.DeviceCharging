using CommunityToolkit.Mvvm.ComponentModel;
using MauiIcons.MaterialSymbols.Rounded;
using Plugin.DeviceCharging.Sample.Models;
using Plugin.DeviceCharging.Sample.Services;

namespace Plugin.DeviceCharging.Sample.ViewModels;

public enum KeepScreenOnMode
{
	Always,
	ReadingOnly,
	OnlyWhenCharging,
	Never
}

public partial class SettingsViewModel : ObservableObject
{
	readonly ISettingsService settingsService;

	public List<SelectOption> ThemeOptions { get; } =
	[
		new("System Preferences", MaterialSymbolsRoundedIcons.SettingsSuggest, AppTheme.Unspecified),
		new("Light", MaterialSymbolsRoundedIcons.LightMode, AppTheme.Light),
		new("Dark", MaterialSymbolsRoundedIcons.DarkMode, AppTheme.Dark)
	];

	public List<SelectOption> KeepScreenOnOptions { get; } =
	[
		new("Always", MaterialSymbolsRoundedIcons.ScreenLockPortrait, KeepScreenOnMode.Always),
		new("Reading Only", MaterialSymbolsRoundedIcons.MenuBook, KeepScreenOnMode.ReadingOnly),
		new("Only When Charging", MaterialSymbolsRoundedIcons.BatteryChargingFull, KeepScreenOnMode.OnlyWhenCharging),
		new("Never", MaterialSymbolsRoundedIcons.ScreenLockRotation, KeepScreenOnMode.Never)
	];

	[ObservableProperty] public partial SelectOption SelectedTheme { get; set; }
	[ObservableProperty] public partial SelectOption SelectedKeepScreenOnMode { get; set; }

	bool isUpdatingThemeFromEvent;

	public SettingsViewModel(ISettingsService settingsService)
	{
		this.settingsService = settingsService;

		var savedTheme = this.settingsService.GetTheme();
		SelectedTheme = ThemeOptions.First(t => (AppTheme)t.Value == savedTheme);

		var savedKeepScreenOnMode = this.settingsService.GetKeepScreenOnMode();
		SelectedKeepScreenOnMode = KeepScreenOnOptions.First(m => (KeepScreenOnMode)m.Value == savedKeepScreenOnMode);

		Application.Current?.RequestedThemeChanged += OnRequestedThemeChanged;
	}

	void OnRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
	{
		var currentSavedTheme = settingsService.GetTheme();
		var matchingOption = ThemeOptions.FirstOrDefault(t => (AppTheme)t.Value == currentSavedTheme);
		if (matchingOption != null && SelectedTheme != matchingOption)
		{
			isUpdatingThemeFromEvent = true;
			SelectedTheme = matchingOption;
			isUpdatingThemeFromEvent = false;
		}
	}

	partial void OnSelectedThemeChanged(SelectOption value)
	{
		if (isUpdatingThemeFromEvent)
		{
			return;
		}

		var theme = (AppTheme)value.Value;
		this.settingsService.SetTheme(theme);
		Application.Current?.UserAppTheme = theme;
	}

	partial void OnSelectedKeepScreenOnModeChanged(SelectOption value)
	{
		this.settingsService.SetKeepScreenOnMode((KeepScreenOnMode)value.Value);
	}
}
