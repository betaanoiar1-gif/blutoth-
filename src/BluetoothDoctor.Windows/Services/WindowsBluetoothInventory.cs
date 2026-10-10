using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
namespace BluetoothDoctor.Windows.Services;
public sealed record BluetoothInventoryResult(bool AdapterDetected, bool? RadioEnabled, int PairedClassicDevices, int PairedBleDevices, IReadOnlyList<string> Notes);
public sealed class WindowsBluetoothInventory
{
    public async Task<BluetoothInventoryResult> ScanAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var notes = new List<string>(); bool detected = false; bool? radio = null; int classicCount = 0, bleCount = 0;
        try {
            var adapter = await BluetoothAdapter.GetDefaultAsync().AsTask(cancellationToken);
            if (adapter is not null) {
                detected = true;
                // Windows' role-support flags do not report the radio's on/off state.
                radio = null;
                notes.Add($"Adapter address: {adapter.BluetoothAddress:X12}");
                notes.Add("Radio power state is not inferred from role-support flags.");
            } else notes.Add("Windows did not return a default Bluetooth adapter; hardware or driver may be unavailable.");
        } catch (Exception ex) { notes.Add($"Adapter query failed: {ex.GetType().Name}: {ex.Message}"); }
        try { classicCount = (await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(true)).AsTask(cancellationToken)).Count; }
        catch (Exception ex) { notes.Add($"Classic device query failed: {ex.GetType().Name}: {ex.Message}"); }
        try { bleCount = (await DeviceInformation.FindAllAsync(BluetoothLEDevice.GetDeviceSelectorFromPairingState(true)).AsTask(cancellationToken)).Count; }
        catch (Exception ex) { notes.Add($"BLE device query failed: {ex.GetType().Name}: {ex.Message}"); }
        return new(detected, radio, classicCount, bleCount, notes);
    }
}