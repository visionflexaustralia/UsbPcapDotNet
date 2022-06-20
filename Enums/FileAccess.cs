// Decompiled with JetBrains decompiler
// Type: USBPcapLib.FileAccess
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

namespace UsbPcapLib.Enums
{
  [Flags]
  public enum FileAccess : uint
  {
    None = 0,
    AccessSystemSecurity = 16777216, // 0x01000000
    MaximumAllowed = 33554432, // 0x02000000
    Delete = 65536, // 0x00010000
    ReadControl = 131072, // 0x00020000
    WriteDAC = 262144, // 0x00040000
    WriteOwner = 524288, // 0x00080000
    Synchronize = 1048576, // 0x00100000
    StandardRightsRequired = WriteOwner | WriteDAC | ReadControl | Delete, // 0x000F0000
    StandardRightsRead = ReadControl, // 0x00020000
    StandardRightsWrite = StandardRightsRead, // 0x00020000
    StandardRightsExecute = StandardRightsWrite, // 0x00020000
    StandardRightsAll = StandardRightsExecute | Synchronize | WriteOwner | WriteDAC | Delete, // 0x001F0000
    SpecificRightsAll = 65535, // 0x0000FFFF
    FILE_READ_DATA = 1,
    FILE_LIST_DIRECTORY = FILE_READ_DATA, // 0x00000001
    FILE_WRITE_DATA = 2,
    FILE_ADD_FILE = FILE_WRITE_DATA, // 0x00000002
    FILE_APPEND_DATA = 4,
    FILE_ADD_SUBDIRECTORY = FILE_APPEND_DATA, // 0x00000004
    FILE_CREATE_PIPE_INSTANCE = FILE_ADD_SUBDIRECTORY, // 0x00000004
    FILE_READ_EA = 8,
    FILE_WRITE_EA = 16, // 0x00000010
    FILE_EXECUTE = 32, // 0x00000020
    FILE_TRAVERSE = FILE_EXECUTE, // 0x00000020
    FILE_DELETE_CHILD = 64, // 0x00000040
    FILE_READ_ATTRIBUTES = 128, // 0x00000080
    FILE_WRITE_ATTRIBUTES = 256, // 0x00000100
    GenericRead = 2147483648, // 0x80000000
    GenericWrite = 1073741824, // 0x40000000
    GenericExecute = 536870912, // 0x20000000
    GenericAll = 268435456, // 0x10000000
    SPECIFIC_RIGHTS_ALL = 65535, // 0x0000FFFF
    FILE_ALL_ACCESS = FILE_WRITE_ATTRIBUTES | FILE_READ_ATTRIBUTES | FILE_DELETE_CHILD | FILE_TRAVERSE | FILE_WRITE_EA | FILE_READ_EA | FILE_CREATE_PIPE_INSTANCE | FILE_ADD_FILE | FILE_LIST_DIRECTORY | StandardRightsAll, // 0x001F01FF
    FILE_GENERIC_READ = FILE_READ_ATTRIBUTES | FILE_READ_EA | FILE_LIST_DIRECTORY | StandardRightsExecute | Synchronize, // 0x00120089
    FILE_GENERIC_WRITE = FILE_WRITE_ATTRIBUTES | FILE_WRITE_EA | FILE_CREATE_PIPE_INSTANCE | FILE_ADD_FILE | StandardRightsExecute | Synchronize, // 0x00120116
    FILE_GENERIC_EXECUTE = FILE_READ_ATTRIBUTES | FILE_TRAVERSE | StandardRightsExecute | Synchronize, // 0x001200A0
  }
}
