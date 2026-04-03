using Plugin.DeviceCharging.Sample.Services;

namespace Plugin.DeviceCharging.Sample;

public partial class App : Application
{
	public App(ISettingsService settingsService)
	{
		InitializeComponent();

		UserAppTheme = settingsService.GetTheme();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell())
		{
			Width = 1200,
			Height = 740,
		};
	}
}