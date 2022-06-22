using System.Runtime.InteropServices;

namespace UsbPcapLib.Structs
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct USB_ENDPOINT_DESCRIPTOR
  {
    public byte bLength;
    public byte bDescriptorType;
    public byte bEndpointAddress;
    public byte bmAttributes;
    public ushort wMaxPacketSize;
    public byte bInterval;
  }
}
