using System.Runtime.InteropServices;
using UsbPcapLib.Structs;

namespace UsbPcapLib;

[StructLayout(LayoutKind.Sequential)]
public struct ThreadData
{
    /// <summary>
    /// Filter device object name
    /// </summary>
    public string device;

    /// <summary>
    /// Output filename
    /// </summary>
    public string filename;

    /// <summary>
    /// Comma separated list with addresses of device to capture
    /// </summary>
    public string address_list;


    /// <summary>
    /// Addresses that should be filtered
    /// </summary>
    public USBPCAP_ADDRESS_FILTER filter;

    /// <summary>
    ///  TRUE if all devices should be captured despite address_list
    /// </summary>
    public bool capture_all;

    /// <summary>
    /// TRUE if we should automatically capture from new devices.
    /// </summary>
    public bool capture_new = true;

    /// <summary>
    /// Snapshot length
    /// </summary>
    public uint snaplen = SafeMethods.DEFAULT_SNAPSHOT_LENGTH;

    /// <summary>
    ///  Internal kernel-mode buffer size
    /// </summary>
    public uint bufferlen = SafeMethods.DEFAULT_INTERNAL_KERNEL_BUFFER_SIZE;

    /// <summary>
    /// FALSE if thread should stop
    /// </summary>
    public volatile bool process = true;

    /// <summary>
    /// Handle to read data from.
    /// </summary>
    public IntPtr read_handle;

    /// <summary>
    /// Handle to read data from.
    /// </summary>
    public IntPtr write_handle;

    /// <summary>
    /// Handle to read data from.
    /// </summary>
    public IntPtr job_handle;

    /// <summary>
    /// Handle to read data from.
    /// </summary>
    public IntPtr worker_process_thread;

    /// <summary>
    /// Handle to event that indicates that main thread should exit.
    /// </summary>
    public EventWaitHandle? exit_event;

    /// <summary>
    /// TRUE if descriptors should be injected into capture.
    /// </summary>
    public bool inject_descriptors;

    public c descriptors;
}

[StructLayout(LayoutKind.Sequential)]
public struct inject_descriptors
{
    /// <summary>
    ///  Packets to inject after pcap header on capture start
    /// pcaprec_hdr_t?
    /// </summary>
    public IntPtr descriptors;

    /// <summary>
    /// inject_packets length in bytes
    /// </summary>
    public int descriptors_len;

    /// <summary>
    /// Buffer to keep track of pcap data read from driver. Once it is filled, the magic
    /// and DLT is checked and if it matches, the the inject_packets are written after
    /// the header and then the normal capture continues.
    /// sizeof(pcaprec_hdr_t)
    /// </summary>
    public unsafe fixed char buf[24];

    public int buf_written;
}
