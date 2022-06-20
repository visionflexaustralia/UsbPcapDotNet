// Decompiled with JetBrains decompiler
// Type: USBPcapLib.USBPcapClient
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;
using UsbPcapLib.Enums;
using UsbPcapLib.Structs;
using FileAccess = UsbPcapLib.Enums.FileAccess;
using FileAttributes = UsbPcapLib.Enums.FileAttributes;
using FileShare = UsbPcapLib.Enums.FileShare;

namespace UsbPcapLib
{
  public class USBPcapClient : IDisposable
  {
    public const int BUFFER_SIZE = 4096;
    private ThreadData _data;
    private int _filterDeviceId;

    public event EventHandler<HeaderEventArgs>? HeaderRead;

    public event EventHandler<DataEventArgs>? DataRead;

    protected virtual void OnHeaderRead(pcap_hdr_t arg)
    {
      var headerRead = this.HeaderRead;
      if (headerRead == null)
      {
          return;
      }

      headerRead(this, new HeaderEventArgs(arg));
    }

    protected virtual void OnDataRead(
      pcaprec_hdr_t arg,
      UsbpcapBufferPacketHeader packet,
      byte[] data)
    {
      var dataRead = this.DataRead;
      if (dataRead == null)
      {
          return;
      }

      dataRead(this, new DataEventArgs(arg, packet, data));
    }

    public USBPcapClient(string filter, int filterDeviceId = 0)
    {
      this._filterDeviceId = filterDeviceId;
      this._data = new ThreadData(filter);
    }

    public void start_capture()
    {
      USBPCAP_ADDRESS_FILTER filter;
      if (!this.USBPcapInitAddressFilter(out filter, this._filterDeviceId))
      {
        Console.WriteLine("USBPcapInitAddressFilter failed!");
      }
      else
      {
        this._data.filter = filter;
        this._data.ExitEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        this._data.read_handle = this.create_filter_read_handle(this._data);
        if (this._data.read_handle == IntPtr.Zero)
        {
            return;
        }

        new Thread(this.read_thread!)
        {
          IsBackground = true
        }.Start(this._data);
      }
    }

    private void read_thread(object obj)
    {
      var data = (ThreadData)obj;
      try
      {
        if (data.read_handle == IntPtr.Zero)
        {
            return;
        }

        var numArray = new byte[(int) data.bufferlen];
        var lpOverlapped1 = new NativeOverlapped();
        var lpOverlapped2 = new NativeOverlapped();
        using (var manualResetEvent1 = new ManualResetEvent(false))
        {
          using (var manualResetEvent2 = new ManualResetEvent(false))
          {
            lpOverlapped1.EventHandle = manualResetEvent1.SafeWaitHandle.DangerousGetHandle();
            lpOverlapped2.EventHandle = manualResetEvent2.SafeWaitHandle.DangerousGetHandle();
            if (SafeMethods.GetFileType(data.read_handle) == FileType.FileTypePipe)
            {
              if (!SafeMethods.ConnectNamedPipe(data.read_handle, ref lpOverlapped2))
              {
                var lastWin32Error = Marshal.GetLastWin32Error();
                if (lastWin32Error != SafeMethods.ERROR_IO_PENDING && lastWin32Error != SafeMethods.ERROR_PIPE_CONNECTED)
                {
                  Console.WriteLine("USBPcapInitAddressFilter failed!");
                  return;
                }
              }
            }
            else
            {
                SafeMethods.ReadFile(data.read_handle, numArray, numArray.Length, out var _, ref lpOverlapped1);
            }

            var waitHandles = new EventWaitHandle[2]
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
                  this.process_data(data, numArray, read);
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
        data.ExitEvent?.Set();
      }
    }

    private unsafe void process_data(ThreadData data, byte[] buffer, uint read)
    {
      using (var input = new MemoryStream(buffer, 0, (int) read))
      {
        using (var br = new BinaryReader(input))
        {
          if (!data.pcapHeaderReadEver)
          {
            pcap_hdr_t pcapHdrT;
            br.TryRead(out pcapHdrT);
            this.OnHeaderRead(pcapHdrT);
            data.pcapHeaderReadEver = true;
          }
          pcaprec_hdr_t pcaprecHdrT;
          UsbpcapBufferPacketHeader packet;
          while (br.TryRead(out pcaprecHdrT) && br.TryRead(out packet))
          {
            var num = sizeof (UsbpcapBufferPacketHeader);
            var count = (int) pcaprecHdrT.incl_len - num;
            var data1 = br.ReadBytes(count);
            this.OnDataRead(pcaprecHdrT, packet, data1);
          }
        }
      }
    }

    public void wait_for_exit_signal() => this._data.ExitEvent?.WaitOne();

    public unsafe IntPtr create_filter_read_handle(ThreadData data)
    {
      var file = SafeMethods.CreateFile(data.device, FileAccess.FILE_GENERIC_WRITE | FileAccess.FILE_READ_DATA | FileAccess.FILE_READ_EA | FileAccess.FILE_READ_ATTRIBUTES, FileShare.None, IntPtr.Zero, FileMode.Open, FileAttributes.Overlapped, IntPtr.Zero);
      if (file == SafeMethods.INVALID_HANDLE_VALUE)
      {
        Console.WriteLine("Couldn't open device");
        return IntPtr.Zero;
      }
      var flag = false;
      try
      {
        var usbpcapIoctlSize = new USBPCAP_IOCTL_SIZE();
        usbpcapIoctlSize.size = data.snaplen;
        var nInBufferSize1 = (uint) sizeof (USBPCAP_IOCTL_SIZE);
        var lpInBuffer1 = new IntPtr(&usbpcapIoctlSize);
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
          var nInBufferSize2 = (uint) sizeof (USBPCAP_ADDRESS_FILTER);
          var lpInBuffer2 = new IntPtr(usbpcapAddressFilterPtr);
          flag = SafeMethods.DeviceIoControl(file, SafeMethods.IOCTL_USBPCAP_START_FILTERING, lpInBuffer2, nInBufferSize2, IntPtr.Zero, 0U, out lpBytesReturned, IntPtr.Zero);
        }


        if (flag)
        {
            return file;
        }

        Console.WriteLine("DeviceIoControl failed (supplimentary code {0})", lpBytesReturned);
        return IntPtr.Zero;
      }
      finally
      {
        if (!flag)
        {
            SafeMethods.CloseHandle(file);
        }
      }
    }

    public bool USBPcapInitAddressFilter(out USBPCAP_ADDRESS_FILTER filter, int filterDeviceId) => this.USBPcapSetDeviceFiltered(out filter, filterDeviceId);

    public unsafe bool USBPcapSetDeviceFiltered(
      out USBPCAP_ADDRESS_FILTER filter,
      int filterDeviceId)
    {
      filter = new USBPCAP_ADDRESS_FILTER();
      byte range;
      byte index;
      if (!this.USBPcapGetAddressRangeAndIndex(filterDeviceId, out range, out index))
      {
          return false;
      }

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
      var output = new StringBuilder();
      var filterHubSymlink = get_usbpcap_filter_hub_symlink(filter);
      if (string.IsNullOrEmpty(filterHubSymlink))
      {
          return output.ToString();
      }

      output.Append("  ");
      output.AppendLine(filterHubSymlink);
      EnumerateHub(filterHubSymlink, new USB_NODE_CONNECTION_INFORMATION?(), 0U, output);
      if (consoleOutput)
      {
          Console.WriteLine(output.ToString());
      }

      return output.ToString();
    }

    private static unsafe void EnumerateHub(
      string hub,
      USB_NODE_CONNECTION_INFORMATION? connection_info,
      uint level,
      StringBuilder output)
    {
      var filename = !hub.StartsWith("\\\\??\\") ? (hub[0] != '\\' ? "\\\\.\\" + hub : hub) : "\\\\.\\" + hub.Substring(4);
      var file = SafeMethods.CreateFile(filename, FileAccess.GenericWrite, FileShare.Write, IntPtr.Zero, FileMode.Open, FileAttributes.None, IntPtr.Zero);
      if (file == SafeMethods.INVALID_HANDLE_VALUE)
      {
          return;
      }

      var num1 = IntPtr.Zero;
      try
      {
        if (file == SafeMethods.INVALID_HANDLE_VALUE)
        {
            output.AppendLine("Couldn't open " + filename);
        }

        var num2 = sizeof (USB_NODE_INFORMATION);
        num1 = Marshal.AllocHGlobal(num2);
        if (!SafeMethods.DeviceIoControl(file, SafeMethods.IOCTL_USB_GET_NODE_INFORMATION, num1, (uint) num2, num1, (uint) num2, out var _, IntPtr.Zero))
        {
            return;
        }

        var structure = Marshal.PtrToStructure<USB_NODE_INFORMATION>(num1);
        EnumerateHubPorts(file, structure.HubInformation.HubDescriptor.bNumberOfPorts, level, !connection_info.HasValue ? (ushort) 0 : connection_info.Value.DeviceAddress, output);
      }
      finally
      {
        SafeMethods.CloseHandle(file);
        if (num1 != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(num1);
        }
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
        var num1 = (uint) sizeof (USB_NODE_CONNECTION_INFORMATION);
        USB_NODE_CONNECTION_INFORMATION connectionInformation;
        connectionInformation.ConnectionIndex = index;
        var num2 = new IntPtr(&connectionInformation);
        if (SafeMethods.DeviceIoControl(hHubDevice, SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_INFORMATION, num2, num1, num2, num1, out var _, IntPtr.Zero) && connectionInformation.ConnectionStatus != USB_CONNECTION_STATUS.NoDeviceConnected)
        {
          var driverKeyName = GetDriverKeyName(hHubDevice, index);
          if (!string.IsNullOrEmpty(driverKeyName))
          {
              PrintDeviceDesc(driverKeyName, index, level, !connectionInformation.DeviceIsHub, connectionInformation.DeviceAddress, hubAddress, output);
          }

          var connectionStatus = (int) connectionInformation.ConnectionStatus;
          if (connectionInformation.DeviceIsHub)
          {
            var externalHubName = GetExternalHubName(hHubDevice, index);
            if (!string.IsNullOrEmpty(externalHubName))
            {
                EnumerateHub(externalHubName, connectionInformation, level + 1U, output);
            }
          }
        }
      }
    }

    private static unsafe string? GetExternalHubName(IntPtr Hub, uint ConnectionIndex)
    {
      var num1 = (uint) sizeof (USB_NODE_CONNECTION_NAME);
      USB_NODE_CONNECTION_NAME nodeConnectionName;
      nodeConnectionName.ConnectionIndex = ConnectionIndex;
      var num2 = new IntPtr(&nodeConnectionName);
      if (!SafeMethods.DeviceIoControl(Hub, SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_NAME, num2, num1, num2, num1, out var _, IntPtr.Zero))
      {
          return string.Empty;
      }

      var lpBytesReturned = nodeConnectionName.ActualLength;
      if (lpBytesReturned <= num1)
      {
          return string.Empty;
      }

      var num3 = Marshal.AllocHGlobal((int) lpBytesReturned);
      try
      {
        Marshal.WriteInt32(num3, (int) ConnectionIndex);
        if (!SafeMethods.DeviceIoControl(Hub, SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_NAME, num3, lpBytesReturned, num3, lpBytesReturned, out lpBytesReturned, IntPtr.Zero))
        {
            return string.Empty;
        }

        var num4 = Marshal.OffsetOf<USB_NODE_CONNECTION_NAME>("NodeName");
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
      {
          output.Append(string.Format("[Port {0}]  (device id: {1}) ", port, deviceAddress));
      }

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
      {
          return;
      }

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
        var numArray = new byte[(int) SafeMethods.MAX_DEVICE_ID_LEN];
        var gcHandle = GCHandle.Alloc(numArray, GCHandleType.Pinned);
        var length1 = (uint) numArray.Length;
        try
        {
          RegistryValueKind pulRegDataType;
          var registryProperty = SafeMethods.CM_Get_DevNode_Registry_Property(pdnDevInst1, 10U, out pulRegDataType, gcHandle.AddrOfPinnedObject(), ref length1, 0U);
          switch (registryProperty)
          {
            case CONFIGRET.CR_SUCCESS:
              var stringAnsi1 = Marshal.PtrToStringAnsi(gcHandle.AddrOfPinnedObject())!;
              if (DriverName.StartsWith(stringAnsi1, StringComparison.OrdinalIgnoreCase))
              {
                var length2 = (uint) numArray.Length;
                if (SafeMethods.CM_Get_DevNode_Registry_Property(pdnDevInst1, 1U, out pulRegDataType, gcHandle.AddrOfPinnedObject(), ref length2, 0U) == CONFIGRET.CR_SUCCESS)
                {
                  var stringAnsi2 = Marshal.PtrToStringAnsi(gcHandle.AddrOfPinnedObject())!;
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
                var sibling = SafeMethods.CM_Get_Sibling(ref pdnDevInst2, pdnDevInst1, 0);
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
      var num1 = parent;
      var level = indent;
      var stack = new Stack<ushort>();
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
        var numArray = new byte[(int) SafeMethods.MAX_DEVICE_ID_LEN];
        var gcHandle = GCHandle.Alloc(numArray, GCHandleType.Pinned);
        var length = (uint) numArray.Length;
        try
        {
          RegistryValueKind pulRegDataType;
          var registryProperty = SafeMethods.CM_Get_DevNode_Registry_Property(num1, 13U, out pulRegDataType, gcHandle.AddrOfPinnedObject(), ref length, 0U);
          if (registryProperty != CONFIGRET.CR_SUCCESS)
          {
            length = (uint) numArray.Length;
            registryProperty = SafeMethods.CM_Get_DevNode_Registry_Property(num1, 1U, out pulRegDataType, gcHandle.AddrOfPinnedObject(), ref length, 0U);
          }
          if (registryProperty == CONFIGRET.CR_SUCCESS)
          {
            var stringAnsi = Marshal.PtrToStringAnsi(gcHandle.AddrOfPinnedObject())!;
            if (!string.IsNullOrEmpty(stringAnsi) && !stack.TryPeek<ushort>(out parentNode))
            {
                parentNode = 0;
            }

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
      var num1 = (uint) sizeof (USB_NODE_CONNECTION_DRIVERKEY_NAME);
      USB_NODE_CONNECTION_DRIVERKEY_NAME connectionDriverkeyName;
      connectionDriverkeyName.ConnectionIndex = index;
      var num2 = new IntPtr(&connectionDriverkeyName);
      uint lpBytesReturned;
      if (!SafeMethods.DeviceIoControl(hHubDevice, SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME, num2, num1, num2, num1, out lpBytesReturned, IntPtr.Zero))
      {
          return string.Empty;
      }

      var actualLength = connectionDriverkeyName.ActualLength;
      if (actualLength <= sizeof (USB_NODE_CONNECTION_DRIVERKEY_NAME))
      {
          return string.Empty;
      }

      var num3 = Marshal.AllocHGlobal((int) actualLength);
      try
      {
        Marshal.WriteInt32(num3, (int) index);
        if (!SafeMethods.DeviceIoControl(hHubDevice, SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME, num3, actualLength, num3, actualLength, out lpBytesReturned, IntPtr.Zero))
        {
            return string.Empty;
        }

        var num4 = Marshal.OffsetOf<USB_NODE_CONNECTION_DRIVERKEY_NAME>("DriverKeyName");
        return Marshal.PtrToStringUni(IntPtr.Add(num3, num4.ToInt32()))!;
      }
      finally
      {
        Marshal.FreeHGlobal(num3);
      }
    }

    private static string? get_usbpcap_filter_hub_symlink(string filter)
    {
      var file = SafeMethods.CreateFile(filter, FileAccess.None, FileShare.None, IntPtr.Zero, FileMode.Open, FileAttributes.None, IntPtr.Zero);
      if (file == SafeMethods.INVALID_HANDLE_VALUE)
      {
        Console.WriteLine("Couldn't open device: " + filter);
        return string.Empty;
      }
      try
      {
        var numArray = new byte[2048];
        var gcHandle = GCHandle.Alloc(numArray, GCHandleType.Pinned);
        try
        {
          return !SafeMethods.DeviceIoControl(file, SafeMethods.IOCTL_USBPCAP_GET_HUB_SYMLINK, IntPtr.Zero, 0U, gcHandle.AddrOfPinnedObject(), (uint) numArray.Length, out var _, IntPtr.Zero) ? string.Empty : Marshal.PtrToStringUni(gcHandle.AddrOfPinnedObject());
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
      using (var registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Class\\{36FC9E60-C465-11CF-8056-444553540000}"))
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
        for (var index = 0; index < strArray.Length; ++index)
        {
          if (strArray[index] == "USBPcap")
          {
              return true;
          }
        }
      }
      return false;
    }

    public static List<string> find_usbpcap_filters()
    {
      var usbpcapFilters = new List<string>();
      var DirectoryHandle = IntPtr.Zero;
      var ObjectAttributes = new ObjectAttributes("\\Device", 0U);
      if (SafeMethods.NtOpenDirectoryObject(out DirectoryHandle, 1U, ref ObjectAttributes) != 0)
      {
          return usbpcapFilters;
      }

      var processHeap = SafeMethods.GetProcessHeap();
      var num = SafeMethods.HeapAlloc(processHeap, 0U, new UIntPtr(4096U));
      try
      {
        uint Context = 0;
        uint ReturnLength = 0;
        if (SafeMethods.NtQueryDirectoryObject(DirectoryHandle, num, 4096, true, true, ref Context, out ReturnLength) != 0)
        {
            return usbpcapFilters;
        }

        while (SafeMethods.NtQueryDirectoryObject(DirectoryHandle, num, 4096, true, false, ref Context, out ReturnLength) == 0)
        {
          var str = "USBPcap";
          var structure = Marshal.PtrToStructure<OBJDIR_INFORMATION>(num);
          if (structure.ObjectName.ToString()?.StartsWith(str) ?? false)
          {
              usbpcapFilters.Add("\\\\.\\" + structure.ObjectName);
          }
        }
      }
      finally
      {
        if (DirectoryHandle != IntPtr.Zero)
        {
            SafeMethods.NtClose(DirectoryHandle);
        }

        if (num != IntPtr.Zero)
        {
            SafeMethods.HeapFree(processHeap, 0U, num);
        }
      }
      return usbpcapFilters;
    }

    public void Dispose()
    {
      if (this._data.ExitEvent != null)
      {
          this._data.ExitEvent.Close();
      }

      if (!(this._data.read_handle != IntPtr.Zero))
      {
          return;
      }

      SafeMethods.CloseHandle(this._data.read_handle);
    }
  }
}
