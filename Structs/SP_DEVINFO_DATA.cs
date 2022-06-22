using System.Runtime.InteropServices;

namespace UsbPcapLib.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct SP_DEVINFO_DATA
{
    public UInt32 cbSize;
    public Guid ClassGuid;
    public IntPtr DevInst;
    public IntPtr Reserved;
}
