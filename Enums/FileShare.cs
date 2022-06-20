// Decompiled with JetBrains decompiler
// Type: USBPcapLib.FileShare
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

namespace USBPcapLib
{
  [Flags]
  public enum FileShare : uint
  {
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 4,
  }
}
