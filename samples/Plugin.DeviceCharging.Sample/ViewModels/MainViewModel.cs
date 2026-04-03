using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Plugin.DeviceCharging.Sample.ViewModels;

public partial class MainViewModel : ObservableObject
{
	[ObservableProperty]
	public partial bool IsRefreshing { get; set; }

	public IRelayCommand RefreshCommand { get; }

	readonly Action reload;

	public MainViewModel(Action reload)
	{
		this.reload = reload;
		RefreshCommand = new RelayCommand(OnRefresh);
	}

	void OnRefresh()
	{
		reload();
		IsRefreshing = false;
	}
}
