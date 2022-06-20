// Decompiled with JetBrains decompiler
// Type: USBPcapLib.UnicodeString
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

using System.Runtime.InteropServices;

namespace USBPcapLib
{
  public struct UnicodeString : IDisposable
  {
    public ushort Length;
    public ushort MaximumLength;
    private IntPtr buffer;

    public UnicodeString(string s)
    {
      Length = (ushort) (s.Length * 2);
      MaximumLength = (ushort) (Length + 2U);
      buffer = Marshal.StringToHGlobalUni(s);
    }

    public void Dispose()
    {
      Marshal.FreeHGlobal(buffer);
      buffer = IntPtr.Zero;
    }

    public override string ToString() => Marshal.PtrToStringUni(buffer);
  }
}
