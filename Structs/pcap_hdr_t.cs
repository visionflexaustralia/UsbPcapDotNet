using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct pcap_hdr_t
{
    public uint magic_number;
    public ushort version_major;
    public ushort version_minor;
    public int thiszone;
    public uint sigfigs;
    public uint snaplen;
    public uint network;
}
