using System.Runtime.InteropServices;

namespace UsbPcapLib;

[StructLayout(LayoutKind.Sequential)]
public struct SP_CLASSINSTALL_HEADER
{
    public UInt32 cbSize;
    public UInt32 InstallFunction;
}
