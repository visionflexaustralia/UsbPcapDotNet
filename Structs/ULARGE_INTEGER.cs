using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Explicit, Size=8)]
struct ULARGE_INTEGER
{
    [FieldOffset(0)]public long QuadPart;

    [FieldOffset(0)]public uint LowPart;
    [FieldOffset(4)]public int HighPart;

    [FieldOffset(0)]public int LowPartAsInt;
    [FieldOffset(0)]public uint LowPartAsUInt;

    [FieldOffset(4)]public int HighPartAsInt;
    [FieldOffset(4)]public uint HighPartAsUInt;
}
