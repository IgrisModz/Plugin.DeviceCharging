namespace Plugin.DeviceCharging;

public partial class ChargingService
{
    public ChargingService()
    {
        SetCharging(false);
    }

    partial void DisposePlatform()
    {
    }
}
