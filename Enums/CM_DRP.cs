// Decompiled with JetBrains decompiler
// Type: USBPcapLib.CM_DRP
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

namespace USBPcapLib
{
  public enum CM_DRP
  {
    CM_DRP_DEVICEDESC = 1,
    CM_DRP_HARDWAREID = 2,
    CM_DRP_COMPATIBLEIDS = 3,
    CM_DRP_UNUSED0 = 4,
    CM_DRP_SERVICE = 5,
    CM_DRP_UNUSED1 = 6,
    CM_DRP_UNUSED2 = 7,
    CM_DRP_CLASS = 8,
    CM_DRP_CLASSGUID = 9,
    CM_DRP_DRIVER = 10, // 0x0000000A
    CM_DRP_CONFIGFLAGS = 11, // 0x0000000B
    CM_DRP_MFG = 12, // 0x0000000C
    CM_DRP_FRIENDLYNAME = 13, // 0x0000000D
    CM_DRP_LOCATION_INFORMATION = 14, // 0x0000000E
    CM_DRP_PHYSICAL_DEVICE_OBJECT_NAME = 15, // 0x0000000F
    CM_DRP_CAPABILITIES = 16, // 0x00000010
    CM_DRP_UI_NUMBER = 17, // 0x00000011
    CM_DRP_UPPERFILTERS = 18, // 0x00000012
  }
}
