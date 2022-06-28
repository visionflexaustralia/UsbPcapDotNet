namespace UsbPcapLib.Enums;

public enum FileType : uint
{
    FileTypeUnknown = 0,
    FileTypeDisk = 1,
    FileTypeChar = 2,
    FileTypePipe = 3,
    FileTypeRemote = 32768 // 0x00008000
}
