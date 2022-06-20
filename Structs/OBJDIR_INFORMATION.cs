// Decompiled with JetBrains decompiler
// Type: USBPcapLib.OBJDIR_INFORMATION
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

using System.Runtime.InteropServices;

namespace UsbPcapLib.Structs
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct OBJDIR_INFORMATION
  {
    public UnicodeString ObjectName;
    public UnicodeString ObjectTypeName;
    public unsafe fixed byte Data[1];
  }
}
