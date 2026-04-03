using Android.Content;
using Android.OS;
using AndroidApp = Android.App.Application;

namespace Plugin.DeviceCharging;

public partial class ChargingService
{
    BatteryReceiver? receiver;

    public ChargingService()
    {
        var filter = new IntentFilter(Intent.ActionBatteryChanged);

        receiver = new BatteryReceiver(this);
        AndroidApp.Context.RegisterReceiver(receiver, filter);
    }

    class BatteryReceiver(ChargingService service) : BroadcastReceiver
    {
        readonly ChargingService service = service;

		public override void OnReceive(Context? context, Intent? intent)
        {
            var status = intent?.GetIntExtra(BatteryManager.ExtraStatus, -1);

            var charging =
                status == (int)BatteryStatus.Charging ||
                status == (int)BatteryStatus.Full;

            service.SetCharging(charging);
        }
    }

    partial void DisposePlatform()
    {
        if (receiver != null)
        {
            AndroidApp.Context.UnregisterReceiver(receiver);
            receiver.Dispose();
            receiver = null;
        }
    }
}