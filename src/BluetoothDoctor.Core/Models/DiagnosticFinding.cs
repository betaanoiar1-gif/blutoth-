namespace BluetoothDoctor.Core.Models;
public enum FindingSeverity { Info, Warning, Error, Critical }
public sealed record DiagnosticFinding(string Code, string Title, string Description, FindingSeverity Severity, string? RecommendedAction = null);
public sealed record DiagnosticSnapshot(bool AdapterDetected, bool AdapterRadioEnabled, bool BluetoothServiceRunning, int PairedDeviceCount, IReadOnlyList<string> Notes);
