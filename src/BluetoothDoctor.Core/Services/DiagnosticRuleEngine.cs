using BluetoothDoctor.Core.Models;
namespace BluetoothDoctor.Core.Services;
public sealed class DiagnosticRuleEngine
{
    public IReadOnlyList<DiagnosticFinding> Analyze(DiagnosticSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        var findings = new List<DiagnosticFinding>();
        if (!snapshot.AdapterDetected)
            findings.Add(new("BT-ADAPTER-MISSING", "Bluetooth adapter not detected", "Windows did not report a usable Bluetooth adapter. Check the adapter, BIOS/UEFI settings, and Device Manager.", FindingSeverity.Error, "Refresh inventory and inspect Device Manager before changing drivers."));
        else if (snapshot.AdapterRadioEnabled == false)
            findings.Add(new("BT-RADIO-OFF", "Bluetooth radio reported unavailable", "An adapter is present, but the platform reports the radio as disabled.", FindingSeverity.Warning, "Check Airplane mode, Windows Bluetooth settings, and hardware wireless switches."));
        if (snapshot.BluetoothServiceRunning == false)
            findings.Add(new("BT-SERVICE-STOPPED", "Bluetooth service is not running", "The platform service check reports that the Bluetooth support service is stopped.", FindingSeverity.Warning, "Inspect service state and event logs; ask before restarting services."));
        if (snapshot.AdapterDetected && snapshot.AdapterRadioEnabled == true && snapshot.PairedDeviceCount == 0)
            findings.Add(new("BT-NO-PAIRED-DEVICES", "No paired devices found", "No paired devices were returned by the current inventory query.", FindingSeverity.Info, "Put the target device in pairing mode and run discovery again."));
        foreach (var note in snapshot.Notes.Where(n => !string.IsNullOrWhiteSpace(n)))
            findings.Add(new("BT-PLATFORM-NOTE", "Platform diagnostic note", note, FindingSeverity.Info));
        if (findings.Count == 0)
            findings.Add(new("BT-BASELINE-OK", "No confirmed basic issue detected", "The available checks found no confirmed basic issue. Some states may be unavailable, and this does not guarantee every Bluetooth profile or device works.", FindingSeverity.Info));
        return findings;
    }
}