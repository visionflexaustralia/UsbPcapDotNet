using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Sequential)]
struct USB_DESCRIPTOR_REQUEST
{
    public int ConnectionIndex;
    public USB_SETUP_PACKET SetupPacket;
    /// <summary>
    /// can be USB_STRING_DESCRIPTOR, <see cref="USB_DEVICE_DESCRIPTOR"/> or <see cref="USB_CONFIGURATION_DESCRIPTOR" />
    /// </summary>
    //public byte[] Data;
}
