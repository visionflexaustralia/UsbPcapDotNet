using UsbPcapLib.Structs;

namespace UsbPcapLib;

public class DataEventArgs : EventArgs
{
    public DataEventArgs(pcaprec_hdr_t record, UsbpcapBufferPacketHeader packetHeader, byte[] data)
    {
        this.Record = record;
        this.Header = packetHeader;
        this.Data = data;
    }

    public pcaprec_hdr_t Record { get; }

    public UsbpcapBufferPacketHeader Header { get; }

    public byte[] Data { get; }
}
