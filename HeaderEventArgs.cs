using UsbPcapLib.Structs;

namespace UsbPcapLib
{
  public class HeaderEventArgs : EventArgs
  {
    private pcap_hdr_t _header;

    public pcap_hdr_t Header => this._header;

    public HeaderEventArgs(pcap_hdr_t header) => this._header = header;
  }
}
