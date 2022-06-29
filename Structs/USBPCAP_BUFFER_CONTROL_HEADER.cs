using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Sequential)]
public struct USBPCAP_BUFFER_CONTROL_HEADER
{
    public UsbpcapBufferPacketHeader header;
    public byte stage;
}


