# Plugin.DeviceCharging

A plugin for .NET MAUI and .NET 10 to detect and monitor the device's charging state.

## Features

- Detects if the device is currently charging (`IsCharging`).
- Subscribes to state changes with the `ChargingStateChanged` event.
- Compatible with Dependency Injection (`IServiceCollection`).

## Installation

Install the `Plugin.DeviceCharging` NuGet package in your .NET MAUI project.

## Configuration (.NET MAUI)

In your `MauiProgram.cs` file, register the `Plugin.DeviceCharging` service:

```csharp
using Plugin.DeviceCharging;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        // Add the DeviceCharging plugin
        builder.Services.AddDeviceCharging();

        return builder.Build();
    }
}
```

## Usage

Inject the `IChargingService` interface into your class / ViewModel.

```csharp
using Plugin.DeviceCharging;

public class MainViewModel
{
    private readonly IChargingService _chargingService;

    // Gets the current charging state
    public bool IsCharging => _chargingService.IsCharging;

    public MainViewModel(IChargingService chargingService)
    {
        _chargingService = chargingService;
        
        // Subscribe to charging state changes
        _chargingService.ChargingStateChanged += OnChargingStateChanged;
    }

    private void OnChargingStateChanged(object? sender, bool e)
    {
        // 'e' indicates the new charging state
        // Notify the view if necessary
    }
}
```

## API

### `IChargingService`

- `bool IsCharging { get; }`: Returns `true` if the device is charging or plugged in.
- `event EventHandler<bool>? ChargingStateChanged`: Fires with the current state (`true` / `false`) when the charging source changes.

## Compatibility

This project targets **.NET 10** and is designed to work on the platforms supported by .NET MAUI (Android, iOS, Windows, MacCatalyst). Platform-specific implementations (such as on Android) automatically handle screen notches and the hiding of system bars.

> **Note regarding iOS and MacCatalyst:**
> These platforms have not been tested. Developing, building, and testing applications for Apple platforms strictly requires Apple-branded hardware (a Mac) according to Apple's End User License Agreement (EULA). Since I do not currently possess the appropriate legal Apple hardware, these platforms remain untested. Contributions, bug reports, and pull requests from developers with macOS environments are highly appreciated!