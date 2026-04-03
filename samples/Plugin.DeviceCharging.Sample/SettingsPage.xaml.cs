using Plugin.DeviceCharging.Sample.ViewModels;

namespace Plugin.DeviceCharging.Sample;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}