﻿// Decompiled with JetBrains decompiler
// Type: USBPcapLib.USBPCAP_ADDRESS_FILTER
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

using System.Runtime.InteropServices;

namespace USBPcapLib
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct USBPCAP_ADDRESS_FILTER
  {
    public unsafe fixed uint addresses[4];
    public bool filterAll;
  }
}
