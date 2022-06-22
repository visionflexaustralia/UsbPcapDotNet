using System.Runtime.InteropServices;

namespace UsbPcapLib.Structs
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct USBPCAP_ADDRESS_FILTER
  {
    public unsafe fixed uint addresses[4];
    public bool filterAll;
  }
}
