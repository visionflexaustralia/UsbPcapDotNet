// Decompiled with JetBrains decompiler
// Type: USBPcapLib.USB_NODE_CONNECTION_DRIVERKEY_NAME
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

namespace USBPcapLib
{
  public struct USB_NODE_CONNECTION_DRIVERKEY_NAME
  {
    public uint ConnectionIndex;
    public uint ActualLength;
    public unsafe fixed char DriverKeyName[1];
  }
}
