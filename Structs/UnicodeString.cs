// Decompiled with JetBrains decompiler
// Type: USBPcapLib.UnicodeString
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

using System.Runtime.InteropServices;

namespace UsbPcapLib.Structs
{
  public struct UnicodeString : IDisposable
  {
    public ushort Length;
    public ushort MaximumLength;
    private IntPtr buffer;

    public UnicodeString(string s)
    {
      this.Length = (ushort) (s.Length * 2);
      this.MaximumLength = (ushort) (this.Length + 2U);
      this.buffer = Marshal.StringToHGlobalUni(s);
    }

    public void Dispose()
    {
      Marshal.FreeHGlobal(this.buffer);
      this.buffer = IntPtr.Zero;
    }

    public override string? ToString() => Marshal.PtrToStringUni(this.buffer);
  }
}
