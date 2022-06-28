using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct USB_PIPE_INFO
{
    public USB_ENDPOINT_DESCRIPTOR EndpointDescriptor;
    public uint ScheduleOffset;
}
