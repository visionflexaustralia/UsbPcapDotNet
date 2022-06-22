using System.Runtime.InteropServices;

namespace UsbPcapLib.Structs
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct pcaprec_hdr_t
  {
    public uint ts_sec;
    public uint ts_usec;
    public uint incl_len;
    public uint orig_len;
  }
}
