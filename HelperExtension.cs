// Decompiled with JetBrains decompiler
// Type: USBPcapLib.HelperExtension
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

using System.Runtime.InteropServices;

namespace UsbPcapLib
{
  internal static class HelperExtension
  {
    public static bool TryPeek<T>(this Stack<T> stack, out T item) where T: struct
    {
      item = default;
      if (stack.Count == 0)
      {
          return false;
      }

      item = stack.Peek();
      return true;
    }

    public static bool TryPop<T>(this Stack<T> stack, out T item) where T: struct
    {
      item = default;
      if (stack.Count == 0)
      {
          return false;
      }

      item = stack.Pop();
      return true;
    }

    public static unsafe bool TryRead<T>(this BinaryReader br, out T value) where T : unmanaged
    {
      var count = sizeof (T);
      value = default (T);
      var numArray = br.ReadBytes(count);
      if (numArray.Length == 0)
      {
          return false;
      }

      var gcHandle = GCHandle.Alloc(numArray, GCHandleType.Pinned);
      try
      {
        value = Marshal.PtrToStructure<T>(gcHandle.AddrOfPinnedObject());
        return true;
      }
      finally
      {
        gcHandle.Free();
      }
    }
  }
}
