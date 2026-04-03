namespace Plugin.DeviceCharging;

public static class DeviceChargingExtensions
{
    public static IServiceCollection AddDeviceCharging(this IServiceCollection services)
    {
        services.AddSingleton<IChargingService, ChargingService>();
        return services;
    }
}