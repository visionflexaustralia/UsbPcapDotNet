namespace UsbPcapLib.Enums;

public enum CreationDisposition : uint
{
    New = 1,
    CreateAlways = 2,
    OpenExisting = 3,
    OpenAlways = 4,
    TruncateExisting = 5
}
