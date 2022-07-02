using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Sequential)]
struct USB_SETUP_PACKET
{
    public byte bmRequest;
    public byte bRequest;
    public ushort wValue;
    public ushort wIndex;
    public ushort wLength;
}
