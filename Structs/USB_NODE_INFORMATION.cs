// Decompiled with JetBrains decompiler
// Type: USBPcapLib.USB_NODE_INFORMATION
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

using System.Runtime.InteropServices;

namespace USBPcapLib
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct USB_NODE_INFORMATION
  {
    [FieldOffset(0)]
    public USB_HUB_NODE NodeType;
    [FieldOffset(4)]
    public USB_HUB_INFORMATION HubInformation;
    [FieldOffset(4)]
    public USB_MI_PARENT_INFORMATION MiParentInformation;
  }
}
