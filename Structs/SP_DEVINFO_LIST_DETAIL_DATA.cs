using System.Runtime.InteropServices;

namespace UsbPcapLib;

[StructLayout(LayoutKind.Sequential)]
public struct SP_DEVINFO_LIST_DETAIL_DATA
{
    public UInt32 cbSize;
    public Guid ClassGuid;
    public IntPtr RemoteMachineHandle;
    public IntPtr RemoteMachineName;
}
