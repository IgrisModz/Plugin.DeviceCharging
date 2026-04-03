using Plugin.DeviceCharging.Sample.ViewModels;

namespace Plugin.DeviceCharging.Sample.Services;

public interface ISettingsService
{
	AppTheme GetTheme();
	void SetTheme(AppTheme theme);

	KeepScreenOnMode GetKeepScreenOnMode();
	void SetKeepScreenOnMode(KeepScreenOnMode mode);
}
