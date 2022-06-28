using System.Runtime.InteropServices;

namespace UsbPcapLib.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct USB_HUB_DESCRIPTOR
{
    public byte bDescriptorLength;
    public byte bDescriptorType;
    public byte bNumberOfPorts;
    public ushort wHubCharacteristics;
    public byte bPowerOnToPowerGood;
    public byte bHubControlCurrent;
    public unsafe fixed byte bRemoveAndPowerMask[64];
}
