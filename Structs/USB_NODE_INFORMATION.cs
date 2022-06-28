using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public struct USB_NODE_INFORMATION
{
    [FieldOffset(0)] public USB_HUB_NODE NodeType;
    [FieldOffset(4)] public USB_HUB_INFORMATION HubInformation;
    [FieldOffset(4)] public USB_MI_PARENT_INFORMATION MiParentInformation;
}
