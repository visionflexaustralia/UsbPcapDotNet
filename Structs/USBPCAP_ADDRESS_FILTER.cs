using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct USBPCAP_ADDRESS_FILTER
{
    public unsafe fixed uint addresses[4];
    public bool filterAll;
}
