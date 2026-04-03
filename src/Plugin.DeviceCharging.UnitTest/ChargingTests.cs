namespace Plugin.DeviceCharging.UnitTest;

public partial class ChargingTests
{
    partial class TestChargingService : ChargingService
    {
        public void UpdateChargingState(bool charging)
        {
            SetCharging(charging);
        }
    }

    [Fact]
    public void InitialState_IsFalse()
    {
        var service = new TestChargingService();
        Assert.False(service.IsCharging);
    }

    [Fact]
    public void UpdateChargingState_ChangesStateAndFiresEvent()
    {
        var service = new TestChargingService();
        bool eventFired = false;
        bool eventState = false;

        service.ChargingStateChanged += (sender, isCharging) =>
        {
            eventFired = true;
            eventState = isCharging;
        };

        service.UpdateChargingState(true);

        Assert.True(service.IsCharging);
        Assert.True(eventFired);
        Assert.True(eventState);
    }

    [Fact]
    public void UpdateChargingState_SameState_DoesNotFireEvent()
    {
        var service = new TestChargingService();
        service.UpdateChargingState(true); 

        bool eventFired = false;
        service.ChargingStateChanged += (sender, isCharging) =>
        {
            eventFired = true;
        };

        service.UpdateChargingState(true);

        Assert.False(eventFired);
    }
}
