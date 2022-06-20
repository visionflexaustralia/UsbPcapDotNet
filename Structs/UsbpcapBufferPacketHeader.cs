// Decompiled with JetBrains decompiler
// Type: USBPcapLib.USBPCAP_BUFFER_PACKET_HEADER
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

using System.Runtime.InteropServices;
using UsbPcapLib.Enums;

namespace UsbPcapLib.Structs
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct UsbpcapBufferPacketHeader
  {
    public ushort headerLen;
    public ulong irpId;
    public USBD_STATUS status;
    public URB_FUNCTION function;
    public byte info;
    public ushort bus;
    public ushort device;
    public byte endpoint;
    public USBPCAP_TRANSFER_TYPE transfer;
    public uint dataLength;

    public int EndpointNumber => this.endpoint & 15;

    public bool In => (this.endpoint & 128) == 128;

    public IRPDierction IrpDirection => (this.info & 1) == 0 ? IRPDierction.FDO_TO_PDO : IRPDierction.PDO_TO_FDO;
  }
}
