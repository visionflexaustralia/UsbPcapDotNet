// Decompiled with JetBrains decompiler
// Type: USBPcapLib.pcap_hdr_t
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

using System.Runtime.InteropServices;

namespace UsbPcapLib.Structs
{
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
}
