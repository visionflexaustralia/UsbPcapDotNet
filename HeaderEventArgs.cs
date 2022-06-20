// Decompiled with JetBrains decompiler
// Type: USBPcapLib.HeaderEventArgs
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

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
