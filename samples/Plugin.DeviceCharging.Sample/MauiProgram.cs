using CommunityToolkit.Maui;
using MauiIcons.Material.Round;
using MauiIcons.MaterialSymbols.Rounded;
using Plugin.DeviceCharging.Sample.Services;
using Plugin.DeviceCharging.Sample.ViewModels;

namespace Plugin.DeviceCharging.Sample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		_ = builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseMaterialRound()
			.UseMaterialSymbolsRounded()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddDeviceCharging();

		builder.Services.AddSingleton<ISettingsService, SettingsService>();
		builder.Services.AddSingleton<SettingsViewModel>();

		return builder.Build();
	}
}
