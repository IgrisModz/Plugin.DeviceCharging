using CommunityToolkit.Mvvm.ComponentModel;

namespace Plugin.DeviceCharging.Sample.Models;

public partial class SelectOption(string label, Enum icon, Enum value) : ObservableObject
{
	public string Label { get; set; } = label;
	public Enum Icon { get; set; } = icon;
	public Enum Value { get; set; } = value;

	public bool HasIcon => Icon != null;

	[ObservableProperty]
	public partial bool IsSelected {  get; set; }
}