using Plugin.DeviceCharging.Sample.Constants;
using Plugin.DeviceCharging.Sample.ViewModels;

namespace Plugin.DeviceCharging.Sample.Services;

public class SettingsService : ISettingsService
{
	public AppTheme GetTheme()
	{
		var themeString = Preferences.Get(PreferenceKeys.SelectedTheme, AppTheme.Unspecified.ToString());
		return Enum.TryParse<AppTheme>(themeString, out var theme) ? theme : AppTheme.Unspecified;
	}

	public void SetTheme(AppTheme theme)
	{
		Preferences.Set(PreferenceKeys.SelectedTheme, theme.ToString());
	}

	public KeepScreenOnMode GetKeepScreenOnMode()
	{
		var modeString = Preferences.Get(PreferenceKeys.SelectedKeepScreenOnMode, KeepScreenOnMode.Never.ToString());
		return Enum.TryParse<KeepScreenOnMode>(modeString, out var mode) ? mode : KeepScreenOnMode.Never;
	}

	public void SetKeepScreenOnMode(KeepScreenOnMode mode)
	{
		Preferences.Set(PreferenceKeys.SelectedKeepScreenOnMode, mode.ToString());
	}
}
