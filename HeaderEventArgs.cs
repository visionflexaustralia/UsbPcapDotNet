namespace UsbPcapDotNet;

public class HeaderEventArgs : EventArgs
{
    public HeaderEventArgs(pcap_hdr_t header)
    {
        this.Header = header;
    }

    public pcap_hdr_t Header { get; }
}
