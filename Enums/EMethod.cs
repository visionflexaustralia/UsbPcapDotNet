namespace UsbPcapLib.Enums;

[Flags]
public enum EMethod : uint
{
    Buffered    = 0,
    InDirect    = 1,
    OutDirect    = 2,
    Neither        = 3
}
