using BluetoothDoctor.Core.Models;
using BluetoothDoctor.Core.Services;
namespace BluetoothDoctor.Core.Tests;
public sealed class DiagnosticRuleEngineTests
{
    private readonly DiagnosticRuleEngine _engine = new();
    [Fact] public void MissingAdapterProducesErrorFinding() { var r = _engine.Analyze(new(false, false, true, 0, Array.Empty<string>())); Assert.Contains(r, f => f.Code == "BT-ADAPTER-MISSING" && f.Severity == FindingSeverity.Error); }
    [Fact] public void DisabledRadioProducesWarning() { var r = _engine.Analyze(new(true, false, true, 1, Array.Empty<string>())); Assert.Contains(r, f => f.Code == "BT-RADIO-OFF"); }
    [Fact] public void CleanBaselineIsQualified() { var r = _engine.Analyze(new(true, true, true, 2, Array.Empty<string>())); Assert.Single(r); Assert.Equal("BT-BASELINE-OK", r[0].Code); Assert.Contains("does not guarantee", r[0].Description); }
    [Fact] public void PlatformNotesArePreserved() { var r = _engine.Analyze(new(true, true, true, 1, new[] { "permission denied" })); Assert.Contains(r, f => f.Code == "BT-PLATFORM-NOTE" && f.Description == "permission denied"); }
}