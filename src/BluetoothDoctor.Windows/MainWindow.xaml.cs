using System.Windows;
using BluetoothDoctor.Core.Models;
using BluetoothDoctor.Core.Services;
using BluetoothDoctor.Windows.Services;
namespace BluetoothDoctor.Windows;
public partial class MainWindow : Window
{
    private readonly WindowsBluetoothInventory _inventory = new();
    private readonly DiagnosticRuleEngine _engine = new();
    public MainWindow() => InitializeComponent();
    private async void ScanButton_Click(object sender, RoutedEventArgs e)
    {
        ScanButton.IsEnabled = false; StatusText.Text = "جارٍ الفحص..."; FindingsList.Items.Clear();
        try {
            var result = await _inventory.ScanAsync();
            AdapterValue.Text = result.AdapterDetected ? "مكتشف" : "غير مكتشف";
            ClassicValue.Text = result.PairedClassicDevices.ToString(); BleValue.Text = result.PairedBleDevices.ToString();
            var snapshot = new DiagnosticSnapshot(result.AdapterDetected, result.RadioEnabled, null, result.PairedClassicDevices + result.PairedBleDevices, result.Notes);
            var findings = _engine.Analyze(snapshot);
            foreach (var finding in findings) FindingsList.Items.Add($"[{finding.Severity}] {finding.Title}\n{finding.Description}");
            SummaryText.Text = $"اكتمل الفحص: {findings.Count} نتيجة. لم يتم تغيير أي إعداد في النظام."; StatusText.Text = "اكتمل الفحص";
        } catch (Exception ex) { SummaryText.Text = $"تعذر إكمال الفحص: {ex.Message}"; StatusText.Text = "تعذر الفحص"; }
        finally { ScanButton.IsEnabled = true; }
    }
}