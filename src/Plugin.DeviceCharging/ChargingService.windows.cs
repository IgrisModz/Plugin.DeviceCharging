using Battery = Windows.Devices.Power.Battery;
using Windows.System.Power;

namespace Plugin.DeviceCharging;

public partial class ChargingService
{
    public ChargingService()
    {
        Battery.AggregateBattery.ReportUpdated += OnBatteryReportUpdated;
        Update();
    }

    void OnBatteryReportUpdated(Battery sender, object args)
    {
        Update();
    }

    void Update()
    {
        var report = Battery.AggregateBattery.GetReport();

		if (report == null)
		{
			return;
		}

        var charging =
            report.Status == BatteryStatus.Charging ||
            report.Status == BatteryStatus.Idle;

        SetCharging(charging);
    }

    partial void DisposePlatform()
    {
        Battery.AggregateBattery.ReportUpdated -= OnBatteryReportUpdated;
    }
}