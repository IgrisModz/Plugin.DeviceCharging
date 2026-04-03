namespace Plugin.DeviceCharging;

public partial class ChargingService : IChargingService
{
    protected bool isCharging;
    bool disposed;

    public bool IsCharging => isCharging;

    public event EventHandler<bool>? ChargingStateChanged;

    protected void SetCharging(bool charging)
    {
        if (isCharging == charging)
        {
            return;
        }

        isCharging = charging;
        ChargingStateChanged?.Invoke(this, charging);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }

        if (disposing)
        {
            // Dispose managed resources
        }

        DisposePlatform();
        disposed = true;
    }

    partial void DisposePlatform();
}