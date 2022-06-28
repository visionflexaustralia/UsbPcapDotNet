namespace UsbPcapLib.Enums;

[Flags]
public enum FileShare : uint
{
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 4
}
