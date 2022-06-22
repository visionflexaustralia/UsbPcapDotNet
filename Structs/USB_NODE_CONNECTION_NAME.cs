using System.Runtime.InteropServices;

namespace UsbPcapLib.Structs
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct USB_NODE_CONNECTION_NAME
  {
    public uint ConnectionIndex;
    public uint ActualLength;
    public unsafe fixed char NodeName[1];
  }
}
