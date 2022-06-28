using System.Runtime.InteropServices;

namespace UsbPcapLib.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct USB_MI_PARENT_INFORMATION
{
    public uint NumberOfInterfaces;
}
