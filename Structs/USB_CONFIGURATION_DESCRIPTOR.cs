using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct USB_CONFIGURATION_DESCRIPTOR
{
    public byte bLength;
    public byte bDescriptorType;
    public ushort wTotalLength;
    public byte bNumInterfaces;
    public byte bConfigurationValue;
    public byte iConfiguration;
    public byte bmAttributes;
    public byte MaxPower;
}
