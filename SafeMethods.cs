
using System.Runtime.InteropServices;
using Microsoft.Win32;
using UsbPcapLib.Enums;
using UsbPcapLib.Structs;
using FileAccess = UsbPcapLib.Enums.FileAccess;
using FileAttributes = UsbPcapLib.Enums.FileAttributes;
using FileShare = UsbPcapLib.Enums.FileShare;

namespace UsbPcapLib
{
  internal class SafeMethods
  {
    internal static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
    internal static uint LOOP_SANITY_LIMIT = 10000;
    internal static uint MAX_DEVICE_ID_LEN = 200;
    internal static uint DEFAULT_SNAPSHOT_LENGTH = ushort.MaxValue;
    internal static uint DEFAULT_INTERNAL_KERNEL_BUFFER_SIZE = 1048576;
    internal static int ERROR_IO_PENDING = 997;
    internal static int ERROR_PIPE_CONNECTED = 535;
    public const uint USB_GET_NODE_INFORMATION = 258;
    public const uint USB_GET_NODE_CONNECTION_INFORMATION = 259;
    public const uint USB_GET_NODE_CONNECTION_NAME = 261;
    public const uint USB_GET_NODE_CONNECTION_DRIVERKEY_NAME = 264;

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool ConnectNamedPipe(
      IntPtr hNamedPipe,
      [In] ref NativeOverlapped lpOverlapped);

    [DllImport("kernel32.dll")]
    internal static extern FileType GetFileType(IntPtr hFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool ReadFile(
      IntPtr hFile,
      byte[] lpBuffer,
      int nNumberOfBytesToRead,
      out uint lpNumberOfBytesRead,
      ref NativeOverlapped lpOverlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool GetOverlappedResult(
      IntPtr hFile,
      [In] ref NativeOverlapped lpOverlapped,
      out uint lpNumberOfBytesTransferred,
      bool bWait);

    [DllImport("ntdll.dll")]
    internal static extern int NtQueryDirectoryObject(
      IntPtr DirectoryHandle,
      IntPtr Buffer,
      int Length,
      bool ReturnSingleEntry,
      bool RestartScan,
      ref uint Context,
      out uint ReturnLength);

    [DllImport("ntdll.dll")]
    internal static extern int NtOpenDirectoryObject(
      out IntPtr DirectoryHandle,
      uint DesiredAccess,
      ref ObjectAttributes ObjectAttributes);

    [DllImport("ntdll.dll")]
    internal static extern int NtClose(IntPtr hObject);

    [DllImport("kernel32.dll")]
    internal static extern IntPtr HeapAlloc(IntPtr hHeap, uint dwFlags, UIntPtr dwBytes);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr GetProcessHeap();

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool HeapFree(IntPtr hHeap, uint dwFlags, IntPtr lpMem);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern IntPtr CreateFile(
      [MarshalAs(UnmanagedType.LPTStr)] string filename,
      [MarshalAs(UnmanagedType.U4)] FileAccess access,
      [MarshalAs(UnmanagedType.U4)] FileShare share,
      IntPtr securityAttributes,
      [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
      [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
      IntPtr templateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool CloseHandle(IntPtr hHandle);

    [DllImport("Kernel32.dll", SetLastError = false, CharSet = CharSet.Auto)]
    public static extern bool DeviceIoControl(
        Microsoft.Win32.SafeHandles.SafeFileHandle hDevice,
        uint IoControlCode,
        IntPtr inBuffer,
        uint nInBufferSize,
        IntPtr OutBuffer,
        uint nOutBufferSize,
        out uint bytesReturned,
        [In] ref NativeOverlapped Overlapped
    );

    [DllImport("setupapi.dll", SetLastError = true)]
    internal static extern CONFIGRET CM_Locate_DevNodeA(
      ref uint pdnDevInst,
      string? pDeviceID,
      int ulFlags);

    [DllImport("setupapi.dll", SetLastError = true)]
    internal static extern CONFIGRET CM_Get_DevNode_Registry_Property(
      uint deviceInstance,
      uint property,
      out RegistryValueKind pulRegDataType,
      IntPtr buffer,
      ref uint length,
      uint flags);

    [DllImport("setupapi.dll")]
    internal static extern CONFIGRET CM_Get_Parent(
      out uint pdnDevInst,
      uint dnDevInst,
      int ulFlags);

    [DllImport("setupapi.dll", SetLastError = true)]
    internal static extern CONFIGRET CM_Get_Child(
      ref uint pdnDevInst,
      uint dnDevInst,
      int ulFlags);

    [DllImport("setupapi.dll", SetLastError = true)]
    internal static extern CONFIGRET CM_Get_Sibling(
      ref uint pdnDevInst,
      uint DevInst,
      int ulFlags);

    [DllImport("setupapi.dll",
        SetLastError = true,
        CharSet = CharSet.Unicode)]
    public static extern unsafe SP_DEVINFO_DATA* SetupDiGetClassDevsEx(ref Guid
            GuidClass,
        [MarshalAs(UnmanagedType.LPWStr)]
        string? Enumerator,
        IntPtr? hParent,
        DIGCF nFlags,
        SP_DEVINFO_DATA* DeviceInfoSet,
        [MarshalAs(UnmanagedType.LPWStr)]
        string? MachineName,
        IntPtr? Reserved);


    internal static int CTL_CODE(int deviceType, int function, int method, int access) => deviceType << 16 | access << 14 | function << 2 | method;

    internal static uint IOCTL_USBPCAP_SETUP_BUFFER => (uint) CTL_CODE(34, 2048, 0, 1);

    internal static uint IOCTL_USBPCAP_START_FILTERING => (uint) CTL_CODE(34, 2049, 0, 3);

    internal static uint IOCTL_USBPCAP_STOP_FILTERING => (uint) CTL_CODE(34, 2050, 0, 3);

    internal static uint IOCTL_USBPCAP_GET_HUB_SYMLINK => (uint) CTL_CODE(34, 2051, 0, 0);

    internal static uint IOCTL_USBPCAP_SET_SNAPLEN_SIZE => (uint) CTL_CODE(34, 2052, 0, 1);

    internal static uint IOCTL_USB_GET_NODE_INFORMATION => (uint) CTL_CODE(34, 258, 0, 0);

    internal static uint IOCTL_USB_GET_NODE_CONNECTION_INFORMATION => (uint) CTL_CODE(34, 259, 0, 0);

    internal static uint IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME => (uint) CTL_CODE(34, 264, 0, 0);

    internal static uint IOCTL_USB_GET_NODE_CONNECTION_NAME => (uint) CTL_CODE(34, 261, 0, 0);

    public static unsafe SP_DEVINFO_DATA* SetupDiGetClassDevsEx(Guid devInterfaceUsbHostControllerGuid, string? enumerator, IntPtr? hwndParent, DIGCF flags, SP_DEVINFO_DATA* deviceInfoSet, string? machineName, IntPtr? reserved)
    {
        return SetupDiGetClassDevsEx(
            ref devInterfaceUsbHostControllerGuid,
            enumerator,
            hwndParent,
            flags,
            deviceInfoSet,
            machineName,
            reserved);
    }

    [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern unsafe bool SetupDiDestroyDeviceInfoList(SP_DEVINFO_DATA* devs);

    [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern unsafe bool SetupDiGetDeviceInfoListDetail(
        SP_DEVINFO_DATA* devs,
        SP_DEVINFO_LIST_DETAIL_DATA* spDevinfoListDetailData);

    [DllImport("setupapi.dll", SetLastError=true)]
    public static extern unsafe bool SetupDiEnumDeviceInfo(SP_DEVINFO_DATA* DeviceInfoSet, uint MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);
  }
}
