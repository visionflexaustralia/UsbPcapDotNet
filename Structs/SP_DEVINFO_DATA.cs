using System.Runtime.InteropServices;

namespace UsbPcapLib.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct SP_DEVINFO_DATA
{
    public UInt32 cbSize;
    public Guid ClassGuid;
    public UInt32 DevInst;
    public IntPtr Reserved;
}
