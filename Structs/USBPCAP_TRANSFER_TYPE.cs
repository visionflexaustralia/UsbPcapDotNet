// Decompiled with JetBrains decompiler
// Type: USBPcapLib.USBPCAP_TRANSFER_TYPE
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

namespace UsbPcapLib.Structs
{
  public enum USBPCAP_TRANSFER_TYPE : byte
  {
    ISOCHRONOUS = 0,
    INTERRUPT = 1,
    CONTROL = 2,
    BULK = 3,
    IRP_INFO = 254, // 0xFE
    UNKNOWN = 255, // 0xFF
  }
}
