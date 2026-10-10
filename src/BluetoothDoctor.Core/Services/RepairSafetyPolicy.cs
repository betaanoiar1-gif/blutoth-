namespace BluetoothDoctor.Core.Services;
public enum RepairRisk { ReadOnly, Low, Disruptive, Destructive }
public sealed record RepairAction(string Id, string Title, RepairRisk Risk, string Impact, bool RequiresAdministrator = false);
public sealed class RepairSafetyPolicy
{
    public bool CanRunAutomatically(RepairAction action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return action.Risk is RepairRisk.ReadOnly or RepairRisk.Low;
    }
    public bool IsAllowed(RepairAction action, bool userConfirmed)
    {
        ArgumentNullException.ThrowIfNull(action);
        return action.Risk is not (RepairRisk.Disruptive or RepairRisk.Destructive) || userConfirmed;
    }
}