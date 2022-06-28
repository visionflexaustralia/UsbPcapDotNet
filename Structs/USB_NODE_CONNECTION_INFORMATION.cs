using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct USB_NODE_CONNECTION_INFORMATION
{
    public uint ConnectionIndex;
    public USB_DEVICE_DESCRIPTOR DeviceDescriptor;
    public byte CurrentConfigurationValue;
    public bool LowSpeed;
    public bool DeviceIsHub;
    public ushort DeviceAddress;
    public uint NumberOfOpenPipes;
    public USB_CONNECTION_STATUS ConnectionStatus;
}
