# UsbPcapDotNet

C# (.NET 6+) binding of [USBPcap](https://github.com/desowin/usbpcap) (specifically USBPcapCMD)

See Example Usage [Here](https://github.com/visionflexaustralia/UsbPcapDotNet/blob/main/UsbPcapDotNet.TestHarness/Program.cs)

```csharp
var filter = USBPcapClient.find_usbpcap_filters().First();
var client = new USBPcapClient(filter);

client.DataRead += (sender, eventArgs) =>
                Console.WriteLine(
                $"Device:'{eventArgs.Header.device}' " +
                $"in?:{eventArgs.Header.In} " +
                $"func:'{eventArgs.Header.function}' " +
                $"len: {eventArgs.Data.Length} ");

client.start_capture();
client.wait_for_exit_signal();
```

## Supports

- [x] Capture USB Packets (NB: requires replugging the device)
- [x] Reset Devices
- [x] Capture specific devices
- [ ] Capture existing devices without re-plug
