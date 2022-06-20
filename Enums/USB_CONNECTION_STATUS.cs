// Decompiled with JetBrains decompiler
// Type: USBPcapLib.USB_CONNECTION_STATUS
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

namespace UsbPcapLib.Enums
{
  public enum USB_CONNECTION_STATUS
  {
    NoDeviceConnected,
    DeviceConnected,
    DeviceFailedEnumeration,
    DeviceGeneralFailure,
    DeviceCausedOvercurrent,
    DeviceNotEnoughPower,
    DeviceNotEnoughBandwidth,
    DeviceHubNestedTooDeeply,
    DeviceInLegacyHub,
    DeviceEnumerating,
    DeviceReset,
  }
}
