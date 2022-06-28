using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct USB_HUB_INFORMATION
{
    public USB_HUB_DESCRIPTOR HubDescriptor;
    public bool HubIsBusPowered;
}
