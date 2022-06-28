using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct USB_MI_PARENT_INFORMATION
{
    public uint NumberOfInterfaces;
}
