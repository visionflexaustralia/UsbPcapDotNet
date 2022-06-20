// Decompiled with JetBrains decompiler
// Type: USBPcapLib.DataEventArgs
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

namespace USBPcapLib
{
  public class DataEventArgs : EventArgs
  {
    private pcaprec_hdr_t _record;
    private UsbpcapBufferPacketHeader _header;
    private byte[] _data;

    public pcaprec_hdr_t Record => _record;

    public UsbpcapBufferPacketHeader Header => _header;

    public byte[] Data => _data;

    public DataEventArgs(
      pcaprec_hdr_t record,
      UsbpcapBufferPacketHeader packetHeader,
      byte[] data)
    {
      _record = record;
      _header = packetHeader;
      _data = data;
    }
  }
}
