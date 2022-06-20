// Decompiled with JetBrains decompiler
// Type: USBPcapLib.USBPcapClient
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace USBPcapLib
{
  public class USBPcapClient : IDisposable
  {
    public const int BUFFER_SIZE = 4096;
    private ThreadData _data;
    private int _filterDeviceId;

    public event EventHandler<HeaderEventArgs> HeaderRead;

    public event EventHandler<DataEventArgs> DataRead;

    protected virtual void OnHeaderRead(pcap_hdr_t arg)
    {
      EventHandler<HeaderEventArgs> headerRead = HeaderRead;
      if (headerRead == null)
        return;
      headerRead(this, new HeaderEventArgs(arg));
    }

    protected virtual void OnDataRead(
      pcaprec_hdr_t arg,
      UsbpcapBufferPacketHeader packet,
      byte[] data)
    {
      EventHandler<DataEventArgs> dataRead = DataRead;
      if (dataRead == null)
        return;
      dataRead(this, new DataEventArgs(arg, packet, data));
    }

    public USBPcapClient(string filter, int filterDeviceId = 0)
    {
      _filterDeviceId = filterDeviceId;
      _data = new ThreadData();
      _data.device = filter;
    }

    public void start_capture()
    {
      USBPCAP_ADDRESS_FILTER filter;
      if (!USBPcapInitAddressFilter(out filter, _filterDeviceId))
      {
        Console.WriteLine("USBPcapInitAddressFilter failed!");
      }
      else
      {
        _data.filter = filter;
        _data.ExitEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        _data.read_handle = create_filter_read_handle(_data);
        if (_data.read_handle == IntPtr.Zero)
          return;
        new Thread(read_thread)
        {
          IsBackground = true
        }.Start(_data);
      }
    }

    private void read_thread(object obj)
    {
      ThreadData data = obj as ThreadData;
      try
      {
        if (data.read_handle == IntPtr.Zero)
          return;
        byte[] numArray = new byte[(int) data.bufferlen];
        NativeOverlapped lpOverlapped1 = new NativeOverlapped();
        NativeOverlapped lpOverlapped2 = new NativeOverlapped();
        using (ManualResetEvent manualResetEvent1 = new ManualResetEvent(false))
        {
          using (ManualResetEvent manualResetEvent2 = new ManualResetEvent(false))
          {
            lpOverlapped1.EventHandle = manualResetEvent1.SafeWaitHandle.DangerousGetHandle();
            lpOverlapped2.EventHandle = manualResetEvent2.SafeWaitHandle.DangerousGetHandle();
            if (SafeMethods.GetFileType(data.read_handle) == FileType.FileTypePipe)
            {
              if (!SafeMethods.ConnectNamedPipe(data.read_handle, ref lpOverlapped2))
              {
                int lastWin32Error = Marshal.GetLastWin32Error();
                if (lastWin32Error != SafeMethods.ERROR_IO_PENDING && lastWin32Error != SafeMethods.ERROR_PIPE_CONNECTED)
                {
                  Console.WriteLine("USBPcapInitAddressFilter failed!");
                  return;
                }
              }
            }
            else
              SafeMethods.ReadFile(data.read_handle, numArray, numArray.Length, out uint _, ref lpOverlapped1);
            EventWaitHandle[] waitHandles = new EventWaitHandle[2]
            {
              manualResetEvent1,
              manualResetEvent2
            };
            uint read;
            while (data.process)
            {
              switch (WaitHandle.WaitAny(waitHandles))
              {
                case 0:
                  SafeMethods.GetOverlappedResult(data.read_handle, ref lpOverlapped1, out read, true);
                  manualResetEvent1.Reset();
                  process_data(data, numArray, read);
                  SafeMethods.ReadFile(data.read_handle, numArray, numArray.Length, out read, ref lpOverlapped1);
                  continue;
                case 1:
                  manualResetEvent2.Reset();
                  SafeMethods.ReadFile(data.read_handle, numArray, numArray.Length, out read, ref lpOverlapped1);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      finally
      {
        data.ExitEvent.Set();
      }
    }

    private unsafe void process_data(ThreadData data, byte[] buffer, uint read)
    {
      using (MemoryStream input = new MemoryStream(buffer, 0, (int) read))
      {
        using (BinaryReader br = new BinaryReader(input))
        {
          if (!data.pcapHeaderReadEver)
          {
            pcap_hdr_t pcapHdrT;
            br.TryRead(out pcapHdrT);
            OnHeaderRead(pcapHdrT);
            data.pcapHeaderReadEver = true;
          }
          pcaprec_hdr_t pcaprecHdrT;
          UsbpcapBufferPacketHeader packet;
          while (br.TryRead(out pcaprecHdrT) && br.TryRead(out packet))
          {
            int num = sizeof (UsbpcapBufferPacketHeader);
            int count = (int) pcaprecHdrT.incl_len - num;
            byte[] data1 = br.ReadBytes(count);
            OnDataRead(pcaprecHdrT, packet, data1);
          }
        }
      }
    }

    public void wait_for_exit_signal() => _data.ExitEvent.WaitOne();

    public unsafe IntPtr create_filter_read_handle(ThreadData data)
    {
      IntPtr file = SafeMethods.CreateFile(data.device, FileAccess.FILE_GENERIC_WRITE | FileAccess.FILE_READ_DATA | FileAccess.FILE_READ_EA | FileAccess.FILE_READ_ATTRIBUTES, FileShare.None, IntPtr.Zero, FileMode.Open, FileAttributes.Overlapped, IntPtr.Zero);
      if (file == SafeMethods.INVALID_HANDLE_VALUE)
      {
        Console.WriteLine("Couldn't open device");
        return IntPtr.Zero;
      }
      bool flag = false;
      try
      {
        USBPCAP_IOCTL_SIZE usbpcapIoctlSize = new USBPCAP_IOCTL_SIZE();
        usbpcapIoctlSize.size = data.snaplen;
        uint nInBufferSize1 = (uint) sizeof (USBPCAP_IOCTL_SIZE);
        IntPtr lpInBuffer1 = new IntPtr(&usbpcapIoctlSize);
        uint lpBytesReturned;
        
        flag = SafeMethods.DeviceIoControl(file, SafeMethods.IOCTL_USBPCAP_SET_SNAPLEN_SIZE, lpInBuffer1, nInBufferSize1, IntPtr.Zero, 0U, out lpBytesReturned, IntPtr.Zero);
        if (!flag)
        {
          Console.WriteLine("DeviceIoControl failed (supplimentary code {0})", lpBytesReturned);
          return IntPtr.Zero;
        }
        usbpcapIoctlSize.size = data.bufferlen;
        
        flag = SafeMethods.DeviceIoControl(file, SafeMethods.IOCTL_USBPCAP_SETUP_BUFFER, lpInBuffer1, nInBufferSize1, IntPtr.Zero, 0U, out lpBytesReturned, IntPtr.Zero);
        if (!flag)
        {
          Console.WriteLine("DeviceIoControl failed (supplimentary code {0})", lpBytesReturned);
          return IntPtr.Zero;
        }
        

        fixed (USBPCAP_ADDRESS_FILTER* usbpcapAddressFilterPtr = &data.filter)
        {
          uint nInBufferSize2 = (uint) sizeof (USBPCAP_ADDRESS_FILTER);
          IntPtr lpInBuffer2 = new IntPtr(usbpcapAddressFilterPtr);
          flag = SafeMethods.DeviceIoControl(file, SafeMethods.IOCTL_USBPCAP_START_FILTERING, lpInBuffer2, nInBufferSize2, IntPtr.Zero, 0U, out lpBytesReturned, IntPtr.Zero);
        }
        

        if (flag)
          return file;
        Console.WriteLine("DeviceIoControl failed (supplimentary code {0})", lpBytesReturned);
        return IntPtr.Zero;
      }
      finally
      {
        if (!flag)
          SafeMethods.CloseHandle(file);
      }
    }

    public bool USBPcapInitAddressFilter(out USBPCAP_ADDRESS_FILTER filter, int filterDeviceId) => USBPcapSetDeviceFiltered(out filter, filterDeviceId);

    public unsafe bool USBPcapSetDeviceFiltered(
      out USBPCAP_ADDRESS_FILTER filter,
      int filterDeviceId)
    {
      filter = new USBPCAP_ADDRESS_FILTER();
      byte range;
      byte index;
      if (!USBPcapGetAddressRangeAndIndex(filterDeviceId, out range, out index))
        return false;
      filter.addresses[range] |= 1U << index;
      return true;
    }

    public bool USBPcapGetAddressRangeAndIndex(int address, out byte range, out byte index)
    {
      range = 0;
      index = 0;
      if (address < 0 || address > sbyte.MaxValue)
      {
        Console.WriteLine("Invalid address: {0}", address);
        return false;
      }
      range = (byte) (address / 32);
      index = (byte) (address % 32);
      return true;
    }

    public static string enumerate_print_usbpcap_interactive(string filter, bool consoleOutput = false)
    {
      StringBuilder output = new StringBuilder();
      string filterHubSymlink = get_usbpcap_filter_hub_symlink(filter);
      if (string.IsNullOrEmpty(filterHubSymlink))
        return output.ToString();
      output.Append("  ");
      output.AppendLine(filterHubSymlink);
      EnumerateHub(filterHubSymlink, new USB_NODE_CONNECTION_INFORMATION?(), 0U, output);
      if (consoleOutput)
        Console.WriteLine(output.ToString());
      return output.ToString();
    }

    private static unsafe void EnumerateHub(
      string hub,
      USB_NODE_CONNECTION_INFORMATION? connection_info,
      uint level,
      StringBuilder output)
    {
      string filename = !hub.StartsWith("\\\\??\\") ? (hub[0] != '\\' ? "\\\\.\\" + hub : hub) : "\\\\.\\" + hub.Substring(4);
      IntPtr file = SafeMethods.CreateFile(filename, FileAccess.GenericWrite, FileShare.Write, IntPtr.Zero, FileMode.Open, FileAttributes.None, IntPtr.Zero);
      if (file == SafeMethods.INVALID_HANDLE_VALUE)
        return;
      IntPtr num1 = IntPtr.Zero;
      try
      {
        if (file == SafeMethods.INVALID_HANDLE_VALUE)
          output.AppendLine("Couldn't open " + filename);
        int num2 = sizeof (USB_NODE_INFORMATION);
        num1 = Marshal.AllocHGlobal(num2);
        if (!SafeMethods.DeviceIoControl(file, SafeMethods.IOCTL_USB_GET_NODE_INFORMATION, num1, (uint) num2, num1, (uint) num2, out uint _, IntPtr.Zero))
          return;
        USB_NODE_INFORMATION structure = Marshal.PtrToStructure<USB_NODE_INFORMATION>(num1);
        EnumerateHubPorts(file, structure.HubInformation.HubDescriptor.bNumberOfPorts, level, !connection_info.HasValue ? (ushort) 0 : connection_info.Value.DeviceAddress, output);
      }
      finally
      {
        SafeMethods.CloseHandle(file);
        if (num1 != IntPtr.Zero)
          Marshal.FreeHGlobal(num1);
      }
    }

    private static unsafe void EnumerateHubPorts(
      IntPtr hHubDevice,
      byte NumPorts,
      uint level,
      ushort hubAddress,
      StringBuilder output)
    {
      for (uint index = 1; index <= NumPorts; ++index)
      {
        uint num1 = (uint) sizeof (USB_NODE_CONNECTION_INFORMATION);
        USB_NODE_CONNECTION_INFORMATION connectionInformation;
        connectionInformation.ConnectionIndex = index;
        IntPtr num2 = new IntPtr(&connectionInformation);
        if (SafeMethods.DeviceIoControl(hHubDevice, SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_INFORMATION, num2, num1, num2, num1, out uint _, IntPtr.Zero) && connectionInformation.ConnectionStatus != USB_CONNECTION_STATUS.NoDeviceConnected)
        {
          string driverKeyName = GetDriverKeyName(hHubDevice, index);
          if (!string.IsNullOrEmpty(driverKeyName))
            PrintDeviceDesc(driverKeyName, index, level, !connectionInformation.DeviceIsHub, connectionInformation.DeviceAddress, hubAddress, output);
          int connectionStatus = (int) connectionInformation.ConnectionStatus;
          if (connectionInformation.DeviceIsHub)
          {
            string externalHubName = GetExternalHubName(hHubDevice, index);
            if (!string.IsNullOrEmpty(externalHubName))
              EnumerateHub(externalHubName, connectionInformation, level + 1U, output);
          }
        }
      }
    }

    private static unsafe string GetExternalHubName(IntPtr Hub, uint ConnectionIndex)
    {
      uint num1 = (uint) sizeof (USB_NODE_CONNECTION_NAME);
      USB_NODE_CONNECTION_NAME nodeConnectionName;
      nodeConnectionName.ConnectionIndex = ConnectionIndex;
      IntPtr num2 = new IntPtr(&nodeConnectionName);
      if (!SafeMethods.DeviceIoControl(Hub, SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_NAME, num2, num1, num2, num1, out uint _, IntPtr.Zero))
        return string.Empty;
      uint lpBytesReturned = nodeConnectionName.ActualLength;
      if (lpBytesReturned <= num1)
        return string.Empty;
      IntPtr num3 = Marshal.AllocHGlobal((int) lpBytesReturned);
      try
      {
        Marshal.WriteInt32(num3, (int) ConnectionIndex);
        if (!SafeMethods.DeviceIoControl(Hub, SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_NAME, num3, lpBytesReturned, num3, lpBytesReturned, out lpBytesReturned, IntPtr.Zero))
          return string.Empty;
        IntPtr num4 = Marshal.OffsetOf<USB_NODE_CONNECTION_NAME>("NodeName");
        return Marshal.PtrToStringUni(IntPtr.Add(num3, num4.ToInt32()));
      }
      finally
      {
        Marshal.FreeHGlobal(num3);
      }
    }

    private static void print_usbpcapcmd(
      uint level,
      uint port,
      string display,
      ushort deviceAddress,
      ushort parentAddress,
      uint node,
      uint parentNode,
      StringBuilder output)
    {
      print_indent(level + 2U, output);
      if (port != 0U)
        output.Append(string.Format("[Port {0}]  (device id: {1}) ", port, deviceAddress));
      output.AppendLine(display ?? "");
    }

    private static void print_indent(uint level, StringBuilder output)
    {
      if (level > 20U)
      {
        output.AppendLine("*** Warning: Device tree might be incorrectly formatted. ***");
      }
      else
      {
        for (; level > 0U; --level)
          output.Append("  ");
      }
    }

    private static void PrintDeviceDesc(
      string DriverName,
      uint index,
      uint Level,
      bool printAllChildren,
      ushort deviceAddress,
      ushort parentAddress,
      StringBuilder output)
    {
      uint pdnDevInst1 = 0;
      uint pdnDevInst2 = 0;
      if (SafeMethods.CM_Locate_DevNodeA(ref pdnDevInst1, null, 0) != CONFIGRET.CR_SUCCESS)
        return;
      uint num1 = 0;
      uint num2 = 0;
label_23:
      while (num2 == 0U)
      {
        if (++num1 > SafeMethods.LOOP_SANITY_LIMIT)
        {
          output.AppendLine("Sanity check failed in PrintDeviceDesc() outer loop!");
          break;
        }
        byte[] numArray = new byte[(int) SafeMethods.MAX_DEVICE_ID_LEN];
        GCHandle gcHandle = GCHandle.Alloc(numArray, GCHandleType.Pinned);
        uint length1 = (uint) numArray.Length;
        try
        {
          RegistryValueKind pulRegDataType;
          CONFIGRET registryProperty = SafeMethods.CM_Get_DevNode_Registry_Property(pdnDevInst1, 10U, out pulRegDataType, gcHandle.AddrOfPinnedObject(), ref length1, 0U);
          switch (registryProperty)
          {
            case CONFIGRET.CR_SUCCESS:
              string stringAnsi1 = Marshal.PtrToStringAnsi(gcHandle.AddrOfPinnedObject());
              if (DriverName.StartsWith(stringAnsi1, StringComparison.OrdinalIgnoreCase))
              {
                uint length2 = (uint) numArray.Length;
                if (SafeMethods.CM_Get_DevNode_Registry_Property(pdnDevInst1, 1U, out pulRegDataType, gcHandle.AddrOfPinnedObject(), ref length2, 0U) == CONFIGRET.CR_SUCCESS)
                {
                  string stringAnsi2 = Marshal.PtrToStringAnsi(gcHandle.AddrOfPinnedObject());
                  print_usbpcapcmd(Level, index, stringAnsi2, deviceAddress, parentAddress, 0U, 0U, output);
                  if (printAllChildren)
                  {
                    PrintDevinstChildren(pdnDevInst1, Level, deviceAddress, output);
                  }
                }
              }

              goto case CONFIGRET.CR_NO_SUCH_VALUE;
            case CONFIGRET.CR_NO_SUCH_VALUE:
              if (SafeMethods.CM_Get_Child(ref pdnDevInst2, pdnDevInst1, 0) == CONFIGRET.CR_SUCCESS)
              {
                pdnDevInst1 = pdnDevInst2;
                continue;
              }
              uint num3 = 0;
              while (++num3 <= SafeMethods.LOOP_SANITY_LIMIT)
              {
                CONFIGRET sibling = SafeMethods.CM_Get_Sibling(ref pdnDevInst2, pdnDevInst1, 0);
                switch (sibling)
                {
                  case CONFIGRET.CR_SUCCESS:
                    pdnDevInst1 = pdnDevInst2;
                    goto label_23;
                  case CONFIGRET.CR_NO_SUCH_DEVNODE:
                    if (SafeMethods.CM_Get_Parent(out pdnDevInst2, pdnDevInst1, 0) == CONFIGRET.CR_SUCCESS)
                    {
                      pdnDevInst1 = pdnDevInst2;
                      continue;
                    }
                    num2 = 1U;
                    goto label_23;
                  default:
                    output.AppendLine(string.Format("CM_Get_Sibling() returned {0}", sibling));
                    return;
                }
              }
              output.AppendLine("Sanity check failed in PrintDeviceDesc() inner loop!");
              return;
            default:
              output.AppendLine(string.Format("Failed to get CM_DRP_DRIVER: {0}", registryProperty));
              return;
          }
        }
        finally
        {
          gcHandle.Free();
        }
      }
    }

    private static void PrintDevinstChildren(
      uint parent,
      uint indent,
      ushort deviceAddress,
      StringBuilder output)
    {
      uint pdnDevInst = 0;
      uint num1 = parent;
      uint level = indent;
      Stack<ushort> stack = new Stack<ushort>();
      ushort parentNode = 0;
      uint num2 = 0;
      ushort node = 1;
      if (SafeMethods.CM_Get_Child(ref pdnDevInst, num1, 0) == CONFIGRET.CR_SUCCESS)
      {
        num1 = pdnDevInst;
        ++level;
        stack.Push(parentNode);
      }
label_23:
      while (level > indent)
      {
        if (++num2 > SafeMethods.LOOP_SANITY_LIMIT)
        {
          output.AppendLine("Sanity check failed in PrintDevinstChildren()");
          break;
        }
        byte[] numArray = new byte[(int) SafeMethods.MAX_DEVICE_ID_LEN];
        GCHandle gcHandle = GCHandle.Alloc(numArray, GCHandleType.Pinned);
        uint length = (uint) numArray.Length;
        try
        {
          RegistryValueKind pulRegDataType;
          CONFIGRET registryProperty = SafeMethods.CM_Get_DevNode_Registry_Property(num1, 13U, out pulRegDataType, gcHandle.AddrOfPinnedObject(), ref length, 0U);
          if (registryProperty != CONFIGRET.CR_SUCCESS)
          {
            length = (uint) numArray.Length;
            registryProperty = SafeMethods.CM_Get_DevNode_Registry_Property(num1, 1U, out pulRegDataType, gcHandle.AddrOfPinnedObject(), ref length, 0U);
          }
          if (registryProperty == CONFIGRET.CR_SUCCESS)
          {
            string stringAnsi = Marshal.PtrToStringAnsi(gcHandle.AddrOfPinnedObject());
            if (!string.IsNullOrEmpty(stringAnsi) && !stack.TryPeek<ushort>(out parentNode))
              parentNode = 0;
            print_usbpcapcmd(level, 0U, stringAnsi, deviceAddress, deviceAddress, node, parentNode, output);
          }
          if (SafeMethods.CM_Get_Child(ref pdnDevInst, num1, 0) == CONFIGRET.CR_SUCCESS)
          {
            num1 = pdnDevInst;
            ++level;
            stack.Push(node);
            ++node;
            continue;
          }
        }
        finally
        {
          gcHandle.Free();
        }
        CONFIGRET sibling;
        do
        {
          sibling = SafeMethods.CM_Get_Sibling(ref pdnDevInst, num1, 0);
          switch (sibling)
          {
            case CONFIGRET.CR_SUCCESS:
              num1 = pdnDevInst;
              ++node;
              goto label_23;
            case CONFIGRET.CR_NO_SUCH_DEVNODE:
              if (SafeMethods.CM_Get_Parent(out pdnDevInst, num1, 0) == CONFIGRET.CR_SUCCESS)
              {
                num1 = pdnDevInst;
                --level;
                parentNode = stack.Pop();
                continue;
              }
              goto label_20;
            default:
              goto label_22;
          }
        }
        while ((int) num1 != (int) parent && (int) level != (int) indent);
        break;
label_20:
        do
          ;
        while (stack.TryPop<ushort>(out parentNode));
        break;
label_22:
        output.AppendLine(string.Format("CM_Get_Sibling() returned {0}", sibling));
        break;
      }
    }

    private static unsafe string GetDriverKeyName(IntPtr hHubDevice, uint index)
    {
      uint num1 = (uint) sizeof (USB_NODE_CONNECTION_DRIVERKEY_NAME);
      USB_NODE_CONNECTION_DRIVERKEY_NAME connectionDriverkeyName;
      connectionDriverkeyName.ConnectionIndex = index;
      IntPtr num2 = new IntPtr(&connectionDriverkeyName);
      uint lpBytesReturned;
      if (!SafeMethods.DeviceIoControl(hHubDevice, SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME, num2, num1, num2, num1, out lpBytesReturned, IntPtr.Zero))
        return string.Empty;
      uint actualLength = connectionDriverkeyName.ActualLength;
      if (actualLength <= sizeof (USB_NODE_CONNECTION_DRIVERKEY_NAME))
        return string.Empty;
      IntPtr num3 = Marshal.AllocHGlobal((int) actualLength);
      try
      {
        Marshal.WriteInt32(num3, (int) index);
        if (!SafeMethods.DeviceIoControl(hHubDevice, SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME, num3, actualLength, num3, actualLength, out lpBytesReturned, IntPtr.Zero))
          return string.Empty;
        IntPtr num4 = Marshal.OffsetOf<USB_NODE_CONNECTION_DRIVERKEY_NAME>("DriverKeyName");
        return Marshal.PtrToStringUni(IntPtr.Add(num3, num4.ToInt32()));
      }
      finally
      {
        Marshal.FreeHGlobal(num3);
      }
    }

    private static string get_usbpcap_filter_hub_symlink(string filter)
    {
      IntPtr file = SafeMethods.CreateFile(filter, FileAccess.None, FileShare.None, IntPtr.Zero, FileMode.Open, FileAttributes.None, IntPtr.Zero);
      if (file == SafeMethods.INVALID_HANDLE_VALUE)
      {
        Console.WriteLine("Couldn't open device: " + filter);
        return string.Empty;
      }
      try
      {
        byte[] numArray = new byte[2048];
        GCHandle gcHandle = GCHandle.Alloc(numArray, GCHandleType.Pinned);
        try
        {
          return !SafeMethods.DeviceIoControl(file, SafeMethods.IOCTL_USBPCAP_GET_HUB_SYMLINK, IntPtr.Zero, 0U, gcHandle.AddrOfPinnedObject(), (uint) numArray.Length, out uint _, IntPtr.Zero) ? string.Empty : Marshal.PtrToStringUni(gcHandle.AddrOfPinnedObject());
        }
        finally
        {
          gcHandle.Free();
        }
      }
      finally
      {
        SafeMethods.CloseHandle(file);
      }
    }

    public static bool is_usbpcap_upper_filter_installed()
    {
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Class\\{36FC9E60-C465-11CF-8056-444553540000}"))
      {
        if (registryKey == null)
        {
          Console.WriteLine("Failed to open USB Class registry key!");
          return false;
        }
        if (!(registryKey.GetValue("UpperFilters", null) is string[] strArray))
        {
          Console.WriteLine("Failed to query UpperFilters value size!");
          return false;
        }
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (strArray[index] == "USBPcap")
            return true;
        }
      }
      return false;
    }

    public static List<string> find_usbpcap_filters()
    {
      List<string> usbpcapFilters = new List<string>();
      IntPtr DirectoryHandle = IntPtr.Zero;
      ObjectAttributes ObjectAttributes = new ObjectAttributes("\\Device", 0U);
      if (SafeMethods.NtOpenDirectoryObject(out DirectoryHandle, 1U, ref ObjectAttributes) != 0)
        return usbpcapFilters;
      IntPtr processHeap = SafeMethods.GetProcessHeap();
      IntPtr num = SafeMethods.HeapAlloc(processHeap, 0U, new UIntPtr(4096U));
      try
      {
        uint Context = 0;
        uint ReturnLength = 0;
        if (SafeMethods.NtQueryDirectoryObject(DirectoryHandle, num, 4096, true, true, ref Context, out ReturnLength) != 0)
          return usbpcapFilters;
        while (SafeMethods.NtQueryDirectoryObject(DirectoryHandle, num, 4096, true, false, ref Context, out ReturnLength) == 0)
        {
          string str = "USBPcap";
          OBJDIR_INFORMATION structure = Marshal.PtrToStructure<OBJDIR_INFORMATION>(num);
          if (structure.ObjectName.ToString().StartsWith(str))
            usbpcapFilters.Add("\\\\.\\" + structure.ObjectName);
        }
      }
      finally
      {
        if (DirectoryHandle != IntPtr.Zero)
          SafeMethods.NtClose(DirectoryHandle);
        if (num != IntPtr.Zero)
          SafeMethods.HeapFree(processHeap, 0U, num);
      }
      return usbpcapFilters;
    }

    public void Dispose()
    {
      if (_data.ExitEvent != null)
        _data.ExitEvent.Close();
      if (!(_data.read_handle != IntPtr.Zero))
        return;
      SafeMethods.CloseHandle(_data.read_handle);
    }
  }
}
