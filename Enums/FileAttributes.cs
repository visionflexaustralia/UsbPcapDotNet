namespace UsbPcapDotNet;

[Flags]
public enum FileAttributes : uint
{
    None = 0,
    Readonly = 1,
    Hidden = 2,
    System = 4,
    Directory = 16, // 0x00000010
    Archive = 32, // 0x00000020
    Device = 64, // 0x00000040
    Normal = 128, // 0x00000080
    Temporary = 256, // 0x00000100
    SparseFile = 512, // 0x00000200
    ReparsePoint = 1024, // 0x00000400
    Compressed = 2048, // 0x00000800
    Offline = 4096, // 0x00001000
    NotContentIndexed = 8192, // 0x00002000
    Encrypted = 16384, // 0x00004000
    Write_Through = 2147483648, // 0x80000000
    Overlapped = 1073741824, // 0x40000000
    NoBuffering = 536870912, // 0x20000000
    RandomAccess = 268435456, // 0x10000000
    SequentialScan = 134217728, // 0x08000000
    DeleteOnClose = 67108864, // 0x04000000
    BackupSemantics = 33554432, // 0x02000000
    PosixSemantics = 16777216, // 0x01000000
    OpenReparsePoint = 2097152, // 0x00200000
    OpenNoRecall = 1048576, // 0x00100000
    FirstPipeInstance = 524288 // 0x00080000
}
