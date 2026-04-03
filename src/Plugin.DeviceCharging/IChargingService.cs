namespace Plugin.DeviceCharging;

public interface IChargingService : IDisposable
{
    event EventHandler<bool>? ChargingStateChanged;

    bool IsCharging { get; }
}
