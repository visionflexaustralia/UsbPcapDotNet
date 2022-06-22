
using UsbPcapLib.Structs;

namespace UsbPcapLib
{
  public class ThreadData
  {
    public string device;
    public USBPCAP_ADDRESS_FILTER filter;
    public EventWaitHandle? ExitEvent;
    public IntPtr read_handle;
    public uint snaplen = SafeMethods.DEFAULT_SNAPSHOT_LENGTH;
    public uint bufferlen = SafeMethods.DEFAULT_INTERNAL_KERNEL_BUFFER_SIZE;
    public bool process = true;
    public bool pcapHeaderReadEver;

    public ThreadData(string device)
    {
        this.device = device;
    }
  }
}
