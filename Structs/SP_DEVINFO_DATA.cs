using System.Runtime.InteropServices;

namespace UsbPcapLib.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct SP_DEVINFO_DATA
{
    public uint cbSize;
    public Guid ClassGuid;
    public uint DevInst;
    public IntPtr Reserved;
}
