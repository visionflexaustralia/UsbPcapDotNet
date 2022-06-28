namespace UsbPcapDotNet;

public struct USB_NODE_CONNECTION_DRIVERKEY_NAME
{
    public uint ConnectionIndex;
    public uint ActualLength;
    public unsafe fixed char DriverKeyName[1];
}
