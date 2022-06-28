// See https://aka.ms/new-console-template for more information

using UsbPcapLib;
using UsbPcapLib.TestHarness;

var filters = USBPcapClient.find_usbpcap_filters();

Console.WriteLine(
    filters.Aggregate((a, b) => a + Environment.NewLine + USBPcapClient.enumerate_print_usbpcap_interactive(b)));

var devices = filters.Select(f => (filterId: f, deviceTree: USBPcapClient.enumerate_print_usbpcap_interactive(f)))
   .OrderByDescending(f => f.deviceTree.Length);

var primary = devices.First();

foreach (var filter in filters)
{
    Console.WriteLine($"Filter: {filter}: ");
    Console.WriteLine();
}

Console.WriteLine($"Choosing Filter: {primary.filterId} ");

var client = new USBPcapClient(primary.filterId);

client.DataRead += (sender, eventArgs) =>
{
    if (eventArgs.Data.Length > 100)
    {
        Console.WriteLine(
            $"DATA READ Device:'{eventArgs.Header.device}' in?:{eventArgs.Header.In} func:'{eventArgs.Header.function}' len: {eventArgs.Data.Length} ");
    }
    else
    {
        Console.WriteLine(
            $"DATA READ Device:'{eventArgs.Header.device}' in?:{eventArgs.Header.In} func:'{eventArgs.Header.function}' len: {eventArgs.Data.Length} data:{eventArgs.Data.ToHexString()}");
    }

    if (eventArgs.Header.In && eventArgs.Data.Length > 0 && Utils.CompareBytes(
            eventArgs.Data,
            new byte[] { 0x02, 0x01, 0x00, 0x00 },
            4))
    {
        Console.WriteLine("CAPTURE!!!!");
    }
};

client.start_capture();

client.wait_for_exit_signal();
