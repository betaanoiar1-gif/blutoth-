# Bluetooth Doctor Pro

A Windows 10/11 Bluetooth diagnostics, repair, device management, and protocol inspection application.

## Project status

Initial repository setup. The first milestone is a safe, testable Windows desktop foundation with a diagnostic core that can be exercised in cloud CI. Hardware-specific Bluetooth tests require Windows and a physical Bluetooth adapter; Google Colab cannot directly access the user's Bluetooth radio or run the WPF desktop UI.

## Planned stack

- C# / .NET 8
- WPF + MVVM desktop UI
- Windows Bluetooth APIs for device enumeration and BLE/GATT
- Safe diagnostic rules and explicit consent for disruptive repairs
- xUnit tests and Windows CI

## Safety principles

- No destructive repair without explicit confirmation.
- Never claim a repair succeeded unless a post-check verifies it.
- No untrusted driver downloads or automatic driver replacement.
- Local-first diagnostics; no telemetry by default.

## Build

Open `BluetoothDoctorPro.sln` in Visual Studio 2022 on Windows with the .NET desktop development workload, or run `dotnet build BluetoothDoctorPro.sln` from a Windows machine with the .NET 8 SDK.