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
