// Decompiled with JetBrains decompiler
// Type: USBPcapLib.FileType
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

namespace USBPcapLib
{
  public enum FileType : uint
  {
    FileTypeUnknown = 0,
    FileTypeDisk = 1,
    FileTypeChar = 2,
    FileTypePipe = 3,
    FileTypeRemote = 32768, // 0x00008000
  }
}
