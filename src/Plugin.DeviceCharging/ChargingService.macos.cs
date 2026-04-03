using Foundation;

using UIKit;

namespace Plugin.DeviceCharging;

public partial class ChargingService
{
    NSObject? observer;

    public ChargingService()
    {
        UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;

        observer = NSNotificationCenter.DefaultCenter.AddObserver(
            UIDevice.BatteryStateDidChangeNotification,
            OnBatteryChanged);

        Update();
    }

    void OnBatteryChanged(NSNotification notification)
    {
        Update();
    }

    void Update()
    {
        var state = UIDevice.CurrentDevice.BatteryState;

        var charging =
            state == UIDeviceBatteryState.Charging ||
            state == UIDeviceBatteryState.Full;

        SetCharging(charging);
    }

    partial void DisposePlatform()
    {
        if (observer != null)
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(observer);
            observer.Dispose();
            observer = null;
        }
    }
}