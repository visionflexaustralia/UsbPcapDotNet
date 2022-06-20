// Decompiled with JetBrains decompiler
// Type: USBPcapLib.OBJECT_ATTRIBUTES
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

using System.Runtime.InteropServices;

namespace USBPcapLib
{
  public struct ObjectAttributes : IDisposable
  {
    public int Length;
    public IntPtr RootDirectory;
    private IntPtr objectName;
    public uint Attributes;
    public IntPtr SecurityDescriptor;
    public IntPtr SecurityQualityOfService;

    public ObjectAttributes(string name, uint attrs)
    {
      Length = 0;
      RootDirectory = IntPtr.Zero;
      objectName = IntPtr.Zero;
      Attributes = attrs;
      SecurityDescriptor = IntPtr.Zero;
      SecurityQualityOfService = IntPtr.Zero;
      Length = Marshal.SizeOf(this);
      ObjectName = new UnicodeString(name);
    }

    public UnicodeString ObjectName
    {
      get => (UnicodeString) Marshal.PtrToStructure(objectName, typeof (UnicodeString));
      set
      {
        bool fDeleteOld = objectName != IntPtr.Zero;
        if (!fDeleteOld)
          objectName = Marshal.AllocHGlobal(Marshal.SizeOf(value));
        Marshal.StructureToPtr(value, objectName, fDeleteOld);
      }
    }

    public void Dispose()
    {
      if (!(objectName != IntPtr.Zero))
        return;
      Marshal.DestroyStructure(objectName, typeof (UnicodeString));
      Marshal.FreeHGlobal(objectName);
      objectName = IntPtr.Zero;
    }
  }
}
