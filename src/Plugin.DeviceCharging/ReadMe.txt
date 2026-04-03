# Plugin.DeviceCharging

Thank you for installing Plugin.DeviceCharging!

To get started, follow these quick steps:

1. Register the service in your .NET MAUI App
Open your MauiProgram.cs file and add AddDeviceCharging() to the service collection:

```csharp
using Plugin.DeviceCharging;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // Add this line
        builder.Services.AddDeviceCharging();

        return builder.Build();
    }
}
```

2. Use the IChargingService
Inject IChargingService into your ViewModels or services to access the charging state:

```csharp
using Plugin.DeviceCharging;

public class MyViewModel
{
    public MyViewModel(IChargingService chargingService)
    {
        // Check if currently charging
        bool isCharging = chargingService.IsCharging;

        // Listen to charging state changes
        chargingService.ChargingStateChanged += (sender, isCurrentlyCharging) =>
        {
            // Handle state change (e.g. notify UI)
        };
    }
}
```

For more details and advanced usage, please refer to the README.md in the project repository.
