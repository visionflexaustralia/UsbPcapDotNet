// Taken from https://github.com/stjeong/usbpcap/tree/master/windows/USBPcapLib
// LICENSE: MIT

using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace UsbPcapDotNet;

public class USBPcapClient : IDisposable
{
    public unsafe delegate void descriptor_callback(
        IntPtr hub,
        ulong port,
        ushort deviceAddress,
        USB_DEVICE_DESCRIPTOR desc,
        descriptor_callback_context* context);

    public const int DIGCF_ALLCLASSES = (0x00000004);
    public const int DIGCF_PRESENT = (0x00000002);
    public const int INVALID_HANDLE_VALUE = -1;
    public const int SPDRP_DEVICEDESC = (0x00000000);
    public const int MAX_DEV_LEN = 1000;
    public const int DEVICE_NOTIFY_WINDOW_HANDLE = (0x00000000);
    public const int DEVICE_NOTIFY_SERVICE_HANDLE = (0x00000001);
    public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = (0x00000004);
    public const int DBT_DEVTYP_DEVICEINTERFACE = (0x00000005);
    public const int DBT_DEVNODES_CHANGED = (0x0007);
    public const int WM_DEVICECHANGE = (0x0219);
    public const int DIF_PROPERTYCHANGE = (0x00000012);
    public const int DICS_FLAG_GLOBAL = (0x00000001);
    public const int DICS_FLAG_CONFIGSPECIFIC = (0x00000002);
    public const int DICS_ENABLE = (0x00000001);
    public const int DICS_DISABLE = (0x00000002);
    public const int DICS_PROPCHANGE = ((0x00000003));
    public const uint ERROR_INVALID_DATA = 13;
    public const uint ERROR_NO_MORE_ITEMS = 259;
    public const uint ERROR_ELEMENT_NOT_FOUND = 1168;

    public const int BUFFER_SIZE = 4096;
    private ThreadData _data;
    private readonly int _filterDeviceId;

    public USBPcapClient(string filter, int filterDeviceId = 0)
    {
        this._filterDeviceId = filterDeviceId;
        this._data = new ThreadData(filter);
    }

    public void Dispose()
    {
        if (this._data.exit_event != null)
        {
            this._data.exit_event.Close();
        }

        if (!(this._data.read_handle != IntPtr.Zero))
        {
            return;
        }

        SafeMethods.CloseHandle(this._data.read_handle);
    }

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

    protected virtual void OnDataRead(pcaprec_hdr_t arg, UsbpcapBufferPacketHeader packet, byte[] data)
    {
        var dataRead = this.DataRead;
        if (dataRead == null)
        {
            return;
        }

        dataRead(this, new DataEventArgs(arg, packet, data));
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
            this._data.exit_event = new EventWaitHandle(false, EventResetMode.ManualReset);
            this._data.read_handle = this.create_filter_read_handle(this._data);
            if (this._data.read_handle == IntPtr.Zero)
            {
                return;
            }

            new Thread(this.read_thread!) { IsBackground = true }.Start(this._data);
        }
    }

    private unsafe IntPtr descriptors_generate_pcap(
        string filter,
        ref int pcap_length,
        USBPCAP_ADDRESS_FILTER addresses)
    {
        var pcap_packets_length = 0;
        var ctx = new descriptor_callback_context();
        ctx.head = null;
        ctx.tail = null;

        this.enumerate_all_connected_devices(null!, null!, null);

        this.generate_pcap_packets(ctx.head, ref pcap_packets_length);

        return IntPtr.Zero;
    }

    private unsafe void enumerate_all_connected_devices(
        string filter,
        descriptor_callback callback,
        descriptor_callback_context* ctx)
    { }

    private unsafe IntPtr generate_pcap_packets(list_entry* ctxHead, ref int pcapPacketsLength)
    {
        throw new NotImplementedException();
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

            var numArray = new byte[(int)data.bufferlen];
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
                            if (lastWin32Error != SafeMethods.ERROR_IO_PENDING
                             && lastWin32Error != SafeMethods.ERROR_PIPE_CONNECTED)
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

                    var waitHandles = new EventWaitHandle[2] { manualResetEvent1, manualResetEvent2 };
                    uint read;
                    while (data.process)
                    {
                        switch (WaitHandle.WaitAny(waitHandles))
                        {
                            case 0:
                                SafeMethods.GetOverlappedResult(data.read_handle, ref lpOverlapped1, out read, true);
                                manualResetEvent1.Reset();
                                this.process_data(data, numArray, read);
                                SafeMethods.ReadFile(
                                    data.read_handle,
                                    numArray,
                                    numArray.Length,
                                    out read,
                                    ref lpOverlapped1);
                                continue;
                            case 1:
                                manualResetEvent2.Reset();
                                SafeMethods.ReadFile(
                                    data.read_handle,
                                    numArray,
                                    numArray.Length,
                                    out read,
                                    ref lpOverlapped1);
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
            data.exit_event?.Set();
        }
    }

    private unsafe void process_data(ThreadData data, byte[] buffer, uint read)
    {
        using (var input = new MemoryStream(buffer, 0, (int)read))
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
                    var num = sizeof(UsbpcapBufferPacketHeader);
                    var count = (int)pcaprecHdrT.incl_len - num;
                    var data1 = br.ReadBytes(count);
                    this.OnDataRead(pcaprecHdrT, packet, data1);
                }
            }
        }
    }

    public void wait_for_exit_signal()
    {
        this._data.exit_event?.WaitOne();
    }

    public unsafe IntPtr create_filter_read_handle(ThreadData data)
    {
        var filter_handle = SafeMethods.CreateFile(
            data.device,
            FileAccess.ReadWrite,
            FileShare.None,
            IntPtr.Zero,
            FileMode.Open,
            FileAttributes.Overlapped,
            IntPtr.Zero);

        if (filter_handle == SafeMethods.INVALID_HANDLE_VALUE)
        {
            Console.WriteLine("Couldn't open device");
            return IntPtr.Zero;
        }

        var success = false;

        try
        {
            var ioctlSize = new USBPCAP_IOCTL_SIZE();
            ioctlSize.size = data.snaplen;
            var inBufSize = (uint)sizeof(USBPCAP_IOCTL_SIZE);

            var pBuf = &ioctlSize;

            var nativeOverlapped = new NativeOverlapped();
            success = SafeMethods.DeviceIoControl(
                filter_handle,
                SafeMethods.IOCTL_USBPCAP_SET_SNAPLEN_SIZE,
                (IntPtr)pBuf,
                inBufSize,
                IntPtr.Zero,
                0,
                out var bytes_ret,
                ref nativeOverlapped);
            if (success == false)
            {
                Console.WriteLine($"DeviceIoControl failed (supplimentary code {bytes_ret}) {Marshal.GetLastWin32Error():x8}");
                return IntPtr.Zero;
            }

            nativeOverlapped = new NativeOverlapped();
            ioctlSize.size = data.bufferlen;
            success = SafeMethods.DeviceIoControl(
                filter_handle,
                SafeMethods.IOCTL_USBPCAP_SETUP_BUFFER,
                (IntPtr)pBuf,
                inBufSize,
                IntPtr.Zero,
                0,
                out bytes_ret,
                ref nativeOverlapped);
            if (success == false)
            {
                Console.WriteLine($"DeviceIoControl failed (supplimentary code {bytes_ret}) {Marshal.GetLastWin32Error():x8}");
                return IntPtr.Zero;
            }

            nativeOverlapped = new NativeOverlapped();
            inBufSize = (uint)sizeof(USBPCAP_ADDRESS_FILTER);
            success = SafeMethods.DeviceIoControl(
                filter_handle,
                SafeMethods.IOCTL_USBPCAP_START_FILTERING,
                (IntPtr)(&data.filter),
                inBufSize,
                IntPtr.Zero,
                0,
                out bytes_ret,
                ref nativeOverlapped);


            if (success == false)
            {
                Console.WriteLine($"DeviceIoControl failed (supplimentary code {bytes_ret}) {Marshal.GetLastWin32Error():x8}");
                return IntPtr.Zero;
            }

            return filter_handle;
        }
        finally
        {
            if (success == false)
            {
                SafeMethods.CloseHandle(filter_handle);
            }
        }
    }

    public bool USBPcapInitAddressFilter(out USBPCAP_ADDRESS_FILTER filter, int filterDeviceId)
    {
        return this.USBPcapSetDeviceFiltered(out filter, filterDeviceId);
    }

    public unsafe bool USBPcapSetDeviceFiltered(out USBPCAP_ADDRESS_FILTER filter, int filterDeviceId)
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

        range = (byte)(address / 32);
        index = (byte)(address % 32);
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
        var deviceName = string.Empty;

        if (hub.StartsWith(@"\\??\"))
        {
            deviceName = @"\\.\" + hub[4..];
        }
        else if (hub[0] == '\\')
        {
            deviceName = hub;
        }
        else
        {
            deviceName = @"\\.\" + hub;
        }

        var hHubDevice = SafeMethods.CreateFile(
            deviceName,
            FileAccess.Write,
            System.IO.FileShare.Write,
            IntPtr.Zero,
            FileMode.Open,
            FileAttributes.Normal,
            IntPtr.Zero);

        if (hHubDevice == SafeMethods.INVALID_HANDLE_VALUE)
        {
            return;
        }

        var pHubInfo = IntPtr.Zero;

        try
        {
            if (hHubDevice == SafeMethods.INVALID_HANDLE_VALUE)
            {
                output.AppendLine("Couldn't open " + deviceName);
            }


            var hubInfoSize = sizeof(USB_NODE_INFORMATION);
            pHubInfo = Marshal.AllocHGlobal(hubInfoSize);

            var overlap = new NativeOverlapped();
            var success = SafeMethods.DeviceIoControl(
                hHubDevice,
                SafeMethods.IOCTL_USB_GET_NODE_INFORMATION,
                pHubInfo,
                (uint)hubInfoSize,
                pHubInfo,
                (uint)hubInfoSize,
                out var nBytes,
                ref overlap);
            if (success == false)
            {
                Console.WriteLine($"Win32 Error: {Marshal.GetLastWin32Error():x8}");
                return;
            }

            var hubInfo = Marshal.PtrToStructure<USB_NODE_INFORMATION>(pHubInfo);

            EnumerateHubPorts(
                hHubDevice,
                hubInfo.HubInformation.HubDescriptor.bNumberOfPorts,
                level,
                connection_info.HasValue == false ? (ushort)0 : connection_info.Value.DeviceAddress,
                output);
        }
        finally
        {
            SafeMethods.CloseHandle(hHubDevice);

            if (pHubInfo != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pHubInfo);
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
            var inBufferSize = (uint)sizeof(USB_NODE_CONNECTION_INFORMATION);
            USB_NODE_CONNECTION_INFORMATION connectionInformation;
            connectionInformation.ConnectionIndex = index;
            var buffer = new IntPtr(&connectionInformation);
            var overlap = new NativeOverlapped();
            if (SafeMethods.DeviceIoControl(
                    hHubDevice,
                    SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_INFORMATION,
                    buffer,
                    inBufferSize,
                    buffer,
                    inBufferSize,
                    out _,
                    ref overlap) && connectionInformation.ConnectionStatus != USB_CONNECTION_STATUS.NoDeviceConnected)
            {
                var driverKeyName = GetDriverKeyName(hHubDevice, index);
                if (!string.IsNullOrEmpty(driverKeyName))
                {
                    PrintDeviceDesc(
                        driverKeyName,
                        index,
                        level,
                        !connectionInformation.DeviceIsHub,
                        connectionInformation.DeviceAddress,
                        hubAddress,
                        output);
                }
                else
                {
                    Console.WriteLine($"Win32 Error: {Marshal.GetLastWin32Error():x8}");
                }

                var connectionStatus = (int)connectionInformation.ConnectionStatus;
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

    private static unsafe string GetExternalHubName(IntPtr Hub, uint ConnectionIndex)
    {
        uint nBytes;
        USB_NODE_CONNECTION_NAME extHubName;
        var nameSize = (uint)sizeof(USB_NODE_CONNECTION_NAME);

        // Get the length of the name of the external hub attached to the
        // specified port.
        extHubName.ConnectionIndex = ConnectionIndex;

        var pInfo = &extHubName;
        var ptrBuf = new IntPtr(pInfo);

        var overlap = new NativeOverlapped();
        var success = SafeMethods.DeviceIoControl(
            Hub,
            SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_NAME,
            ptrBuf,
            nameSize,
            ptrBuf,
            nameSize,
            out nBytes,
            ref overlap);
        if (success == false)
        {
            Console.WriteLine($"Win32 Error: {Marshal.GetLastWin32Error():x8}");
            return string.Empty;
        }

        // Allocate space to hold the external hub name
        nBytes = extHubName.ActualLength;

        if (nBytes <= nameSize)
        {
            return string.Empty;
        }

        var extHubNameW = Marshal.AllocHGlobal((int)nBytes);

        try
        {
            Marshal.WriteInt32(extHubNameW, (int)ConnectionIndex);
            // Get the name of the external hub attached to the specified port

            overlap = new NativeOverlapped();
            success = SafeMethods.DeviceIoControl(
                Hub,
                SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_NAME,
                extHubNameW,
                nBytes,
                extHubNameW,
                nBytes,
                out nBytes,
                ref overlap);

            if (!success)
            {
                Console.WriteLine($"Win32 Error: {Marshal.GetLastWin32Error():x8}");
                return string.Empty;
            }

            // Convert the External Hub name
            var offset = Marshal.OffsetOf<USB_NODE_CONNECTION_NAME>(nameof(extHubName.NodeName));
            return Marshal.PtrToStringUni(IntPtr.Add(extHubNameW, offset.ToInt32()))!;
        }
        finally
        {
            Marshal.FreeHGlobal(extHubNameW);
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
            {
                output.Append("  ");
            }
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
        uint devInst = 0;
        uint devInstNext = 0;

        var cr = SafeMethods.CM_Locate_DevNodeA(ref devInst, null, 0);
        if (cr != CONFIGRET.CR_SUCCESS)
        {
            return;
        }

        uint sanityOuter = 0;
        uint sanityInner = 0;

        uint walkDone = 0;
        while (walkDone == 0)
        {
            if (++sanityOuter > SafeMethods.LOOP_SANITY_LIMIT)
            {
                output.AppendLine("Sanity check failed in PrintDeviceDesc() outer loop!");
                return;
            }

            var buf = new byte[SafeMethods.MAX_DEVICE_ID_LEN];
            var gcHandle = GCHandle.Alloc(buf, GCHandleType.Pinned);
            var len = (uint)buf.Length;
            try
            {
                cr = SafeMethods.CM_Get_DevNode_Registry_Property(
                    devInst,
                    (uint)CM_DRP.CM_DRP_DRIVER,
                    out _,
                    gcHandle.AddrOfPinnedObject(),
                    ref len,
                    0);
                if (cr == CONFIGRET.CR_SUCCESS)
                {
                    var devNodeName = Marshal.PtrToStringAnsi(gcHandle.AddrOfPinnedObject())!;
                    if (DriverName.StartsWith(devNodeName, StringComparison.OrdinalIgnoreCase))
                    {
                        len = (uint)buf.Length;
                        cr = SafeMethods.CM_Get_DevNode_Registry_Property(
                            devInst,
                            (uint)CM_DRP.CM_DRP_DEVICEDESC,
                            out _,
                            gcHandle.AddrOfPinnedObject(),
                            ref len,
                            0);

                        if (cr == CONFIGRET.CR_SUCCESS)
                        {
                            var deviceDesc = Marshal.PtrToStringAnsi(gcHandle.AddrOfPinnedObject())!;

                            print_usbpcapcmd(Level, index, deviceDesc, deviceAddress, parentAddress, 0, 0, output);

                            if (printAllChildren)
                            {
                                PrintDevinstChildren(devInst, Level, deviceAddress, output);
                            }
                        }
                    }
                }
                else if (cr == CONFIGRET.CR_NO_SUCH_VALUE)
                {
                    // No Driver name, it's ok
                }
                else
                {
                    output.AppendLine($"Failed to get CM_DRP_DRIVER: {cr}");
                    return;
                }

                cr = SafeMethods.CM_Get_Child(ref devInstNext, devInst, 0);
                if (cr == CONFIGRET.CR_SUCCESS)
                {
                    devInst = devInstNext;
                    continue;
                }

                sanityInner = 0;

                while (true)
                {
                    if (++sanityInner > SafeMethods.LOOP_SANITY_LIMIT)
                    {
                        output.AppendLine("Sanity check failed in PrintDeviceDesc() inner loop!");
                        return;
                    }

                    cr = SafeMethods.CM_Get_Sibling(ref devInstNext, devInst, 0);

                    if (cr == CONFIGRET.CR_SUCCESS)
                    {
                        devInst = devInstNext;
                        break;
                    }

                    if (cr == CONFIGRET.CR_NO_SUCH_DEVNODE)
                    {
                        // Device doesn't have siblings, go up and try again
                        cr = SafeMethods.CM_Get_Parent(out devInstNext, devInst, 0);

                        if (cr == CONFIGRET.CR_SUCCESS)
                        {
                            devInst = devInstNext;
                        }
                        else
                        {
                            walkDone = 1;
                            break;
                        }
                    }
                    else
                    {
                        output.AppendLine($"CM_Get_Sibling() returned {cr}");
                        return;
                    }
                }
            }
            finally
            {
                gcHandle.Free();
            }
        }
    }

    private static void PrintDevinstChildren(uint parent, uint indent, ushort deviceAddress, StringBuilder output)
    {
        uint next = 0;
        var current = parent;
        var level = indent;
        var nodeStack = new Stack<ushort>();
        ushort parentNode = 0;
        uint sanityCounter = 0;
        ushort nextNode = 1;

        var cr = SafeMethods.CM_Get_Child(ref next, current, 0);
        if (cr == CONFIGRET.CR_SUCCESS)
        {
            current = next;
            level++;
            nodeStack.Push(parentNode);
        }

        while (level > indent)
        {
            if (++sanityCounter > SafeMethods.LOOP_SANITY_LIMIT)
            {
                output.AppendLine("Sanity check failed in PrintDevinstChildren()");
                return;
            }

            var buf = new byte[SafeMethods.MAX_DEVICE_ID_LEN];
            var gcHandle = GCHandle.Alloc(buf, GCHandleType.Pinned);
            var len = (uint)buf.Length;

            try
            {
                cr = SafeMethods.CM_Get_DevNode_Registry_Property(
                    current,
                    (uint)CM_DRP.CM_DRP_FRIENDLYNAME,
                    out _,
                    gcHandle.AddrOfPinnedObject(),
                    ref len,
                    0);
                if (cr != CONFIGRET.CR_SUCCESS)
                {
                    len = (uint)buf.Length;
                    /* Failed to get friendly name,
                     * display device description instead */
                    cr = SafeMethods.CM_Get_DevNode_Registry_Property(
                        current,
                        (uint)CM_DRP.CM_DRP_DEVICEDESC,
                        out _,
                        gcHandle.AddrOfPinnedObject(),
                        ref len,
                        0);
                }

                if (cr == CONFIGRET.CR_SUCCESS)
                {
                    var deviceDesc = Marshal.PtrToStringAnsi(gcHandle.AddrOfPinnedObject())!;
                    if (string.IsNullOrEmpty(deviceDesc) == false)
                    {
                        if (nodeStack.TryPeek(out parentNode) == false)
                        {
                            parentNode = 0;
                        }
                    }

                    print_usbpcapcmd(level, 0, deviceDesc, deviceAddress, deviceAddress, nextNode, parentNode, output);
                }

                // Go down a level to the first next.
                cr = SafeMethods.CM_Get_Child(ref next, current, 0);

                if (cr == CONFIGRET.CR_SUCCESS)
                {
                    current = next;
                    level++;
                    nodeStack.Push(nextNode);
                    nextNode++;
                    continue;
                }
            }
            finally
            {
                gcHandle.Free();
            }

            // Can't go down any further, go across to the next sibling.  If
            // there are no more siblings, go back up until there is a sibling.
            // If we can't go up any further, we're back at the root and we're
            // done.
            while (true)
            {
                cr = SafeMethods.CM_Get_Sibling(ref next, current, 0);

                if (cr == CONFIGRET.CR_SUCCESS)
                {
                    current = next;
                    nextNode++;
                    break;
                }

                if (cr == CONFIGRET.CR_NO_SUCH_DEVNODE)
                {
                    cr = SafeMethods.CM_Get_Parent(out next, current, 0);

                    if (cr == CONFIGRET.CR_SUCCESS)
                    {
                        current = next;
                        level--;
                        parentNode = nodeStack.Pop();
                        if (current == parent || level == indent)
                        {
                            /* We went back to the parent, explicitly return here */
                            return;
                        }
                    }
                    else
                    {
                        while (nodeStack.TryPop(out parentNode)) ;
                        /* Nothing left to do */
                        return;
                    }
                }
                else
                {
                    output.AppendLine($"CM_Get_Sibling() returned {cr}");
                    return;
                }
            }
        }
    }

    private static unsafe string GetDriverKeyName(IntPtr hHubDevice, uint index)
    {
        var num1 = (uint)sizeof(USB_NODE_CONNECTION_DRIVERKEY_NAME);
        USB_NODE_CONNECTION_DRIVERKEY_NAME connectionDriverkeyName;
        connectionDriverkeyName.ConnectionIndex = index;
        var num2 = new IntPtr(&connectionDriverkeyName);
        uint lpBytesReturned;
        var overlap = new NativeOverlapped();
        if (!SafeMethods.DeviceIoControl(
                hHubDevice,
                SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME,
                num2,
                num1,
                num2,
                num1,
                out lpBytesReturned,
                ref overlap))
        {
            Console.WriteLine($"Win32 Error: {Marshal.GetLastWin32Error():x8}");
            return string.Empty;
        }

        var actualLength = connectionDriverkeyName.ActualLength;
        if (actualLength <= sizeof(USB_NODE_CONNECTION_DRIVERKEY_NAME))
        {
            return string.Empty;
        }

        var num3 = Marshal.AllocHGlobal((int)actualLength);
        try
        {
            overlap = new NativeOverlapped();
            Marshal.WriteInt32(num3, (int)index);
            if (!SafeMethods.DeviceIoControl(
                    hHubDevice,
                    SafeMethods.IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME,
                    num3,
                    actualLength,
                    num3,
                    actualLength,
                    out lpBytesReturned,
                    ref overlap))
            {
                Console.WriteLine($"Win32 Error: {Marshal.GetLastWin32Error():x8}");
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
        var file = SafeMethods.CreateFile(
            filter,
            FileAccess.Read,
            FileShare.None,
            IntPtr.Zero,
            FileMode.Open,
            FileAttributes.None,
            IntPtr.Zero);

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
                var overlap = new NativeOverlapped();
                return !SafeMethods.DeviceIoControl(
                    file,
                    SafeMethods.IOCTL_USBPCAP_GET_HUB_SYMLINK,
                    IntPtr.Zero,
                    0U,
                    gcHandle.AddrOfPinnedObject(),
                    (uint)numArray.Length,
                    out var _,
                    ref overlap)
                    ? string.Empty
                    : Marshal.PtrToStringUni(gcHandle.AddrOfPinnedObject());
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
        using (var registryKey = Registry.LocalMachine.OpenSubKey(
                   "System\\CurrentControlSet\\Control\\Class\\{36FC9E60-C465-11CF-8056-444553540000}"))
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
            if (SafeMethods.NtQueryDirectoryObject(
                    DirectoryHandle,
                    num,
                    4096,
                    true,
                    true,
                    ref Context,
                    out ReturnLength) != 0)
            {
                return usbpcapFilters;
            }

            while (SafeMethods.NtQueryDirectoryObject(
                       DirectoryHandle,
                       num,
                       4096,
                       true,
                       false,
                       ref Context,
                       out ReturnLength) == 0)
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

    public static unsafe void foreach_host_controller(
        Action<SafeFileHandle, SP_DEVINFO_DATA, SP_DEVINFO_LIST_DETAIL_DATA_W> callback)
    {
        var devs = (SP_DEVINFO_DATA*)SafeMethods.INVALID_HANDLE_VALUE;
        uint devIndex;
        var devInfo = new SP_DEVINFO_DATA();
        var devInfoListDetail = new SP_DEVINFO_LIST_DETAIL_DATA_W();

        try
        {
            var DevInterfaceUsbHostControllerGuid = Guid.Parse("3ABF6F2D-71C4-462A-8A92-1E6861E6AF27");

            devs = SafeMethods.SetupDiGetClassDevsEx(
                DevInterfaceUsbHostControllerGuid,
                null,
                IntPtr.Zero,
                DIGCF.DIGCF_DEVICEINTERFACE | DIGCF.DIGCF_PRESENT,
                null,
                null,
                IntPtr.Zero);

            if ((IntPtr)devs == SafeMethods.INVALID_HANDLE_VALUE)
            {
                throw new ApplicationException("SetupDiCreateDeviceInfoListEx Failed");
            }

            devInfoListDetail.cbSize = (uint)Marshal.SizeOf<SP_DEVINFO_LIST_DETAIL_DATA_W>();

            if (!SafeMethods.SetupDiGetDeviceInfoListDetailW(devs, ref devInfoListDetail))
            {
                var error = Marshal.GetLastWin32Error();
                var hr = Marshal.GetHRForLastWin32Error();
                throw new ApplicationException($"SetupDiGetDeviceInfoListDetail Failed: err:{error:x8} HR:{hr:x8}");
            }

            devInfo.cbSize = (uint)sizeof(SP_DEVINFO_DATA);
            for (devIndex = 0; SafeMethods.SetupDiEnumDeviceInfo(devs, devIndex, ref devInfo); devIndex++)
            {
                callback(new SafeFileHandle((IntPtr)devs, false), devInfo, devInfoListDetail);
            }
        }
        finally
        {
            SafeMethods.SetupDiDestroyDeviceInfoList(devs);
        }
    }

    private const int MAX_DEVICE_ID_LEN = 200;

    public static unsafe void restart_device(
        SafeFileHandle devs,
        SP_DEVINFO_DATA devInfo,
        SP_DEVINFO_LIST_DETAIL_DATA_W devInfoListDetail)
    {
        var pcp = new SP_PROPCHANGE_PARAMS();
        var devParams = new SP_DEVINSTALL_PARAMS();
        var devID = string.Empty;

        //var handle = GCHandle.Alloc(devID);
        try
        {

            if (SafeMethods.CM_Get_Device_ID_Ex((IntPtr)(&devInfo.DevInst),
                    ref devID,
                    MAX_DEVICE_ID_LEN,
                    0,
                    devInfoListDetail.RemoteMachineHandle) != CONFIGRET.CR_SUCCESS)
            {
                devID = '\0' + devID[1..];
            }
            else
            { }
        }
        finally
        {
            //handle.Free();
        }

        pcp.ClassInstallHeader.cbSize = (uint)sizeof(SP_CLASSINSTALL_HEADER);
        pcp.ClassInstallHeader.InstallFunction = DIF_PROPERTYCHANGE;
        pcp.StateChange = DICS_PROPCHANGE;
        pcp.Scope = DICS_FLAG_CONFIGSPECIFIC;
        pcp.HwProfile = 0;

        if (!SafeMethods.SetupDiSetClassInstallParams(devs, devInfo, pcp.ClassInstallHeader, (uint)sizeof(SP_PROPCHANGE_PARAMS)) ||
            !SafeMethods.SetupDiCallClassInstaller(DIF_PROPERTYCHANGE, devs, devInfo))
        {
            Console.WriteLine("Failed to invoke DIF_PROPERTYCHANGE! Please reboot.\n");
        }
        else
        {
            devParams.cbSize = GetSizeOf(devParams);

            if (SafeMethods.SetupDiGetDeviceInstallParams(devs, devInfo, ref devParams) &&
                (devParams.Flags & (int)(SP_DEVINSTALL_PARAMS_FLAGS.DI_NEEDRESTART | SP_DEVINSTALL_PARAMS_FLAGS.DI_NEEDREBOOT)) != 0)
            {
                Console.WriteLine("Reboot required.\n");
            }
            else
            {
                Console.WriteLine("Restarted.\n");
            }
        }
    }

    public static unsafe int GetSizeOf<T>(T obj) where T : unmanaged
    {
        return sizeof(T);
    }
}
