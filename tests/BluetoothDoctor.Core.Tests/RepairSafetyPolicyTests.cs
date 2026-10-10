using BluetoothDoctor.Core.Services;
namespace BluetoothDoctor.Core.Tests;
public sealed class RepairSafetyPolicyTests
{
    private readonly RepairSafetyPolicy _policy = new();
    [Theory][InlineData(RepairRisk.ReadOnly, true)][InlineData(RepairRisk.Low, true)][InlineData(RepairRisk.Disruptive, false)][InlineData(RepairRisk.Destructive, false)]
    public void AutomaticExecutionOnlyAllowsLowRisk(RepairRisk risk, bool expected) { var action = new RepairAction("test", "Test action", risk, "Impact"); Assert.Equal(expected, _policy.CanRunAutomatically(action)); }
    [Fact] public void DisruptiveActionRequiresConfirmation() { var action = new RepairAction("restart", "Restart service", RepairRisk.Disruptive, "Bluetooth may disconnect."); Assert.False(_policy.IsAllowed(action, false)); Assert.True(_policy.IsAllowed(action, true)); }
}