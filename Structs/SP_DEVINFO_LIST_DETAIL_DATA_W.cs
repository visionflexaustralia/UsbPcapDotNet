using System.Runtime.InteropServices;

namespace UsbPcapLib;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
public struct SP_DEVINFO_LIST_DETAIL_DATA_W
{
    public UInt32 cbSize;
    public Guid ClassGuid;
    public IntPtr RemoteMachineHandle;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string RemoteMachineName;
}
