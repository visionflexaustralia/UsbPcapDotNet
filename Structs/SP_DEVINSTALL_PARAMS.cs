using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Sequential)]
public struct SP_DEVINSTALL_PARAMS
{
    public int cbSize;
    public int Flags;
    public int FlagsEx;
    public readonly IntPtr hwndParent;
    public readonly IntPtr InstallMsgHandler;
    public readonly IntPtr InstallMsgHandlerContext;
    public readonly IntPtr FileQueue;
    public readonly IntPtr ClassInstallReserved;
    public readonly UIntPtr Reserved;
    //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public unsafe char** DriverPath;
}
