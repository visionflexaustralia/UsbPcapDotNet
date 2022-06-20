// Decompiled with JetBrains decompiler
// Type: USBPcapLib.thread_data
// Assembly: USBPcapLib, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B561C15-9FD4-4B20-805D-197561BAD532
// Assembly location: C:\Users\benp\Downloads\USBPcapLib.dll

using UsbPcapLib.Structs;

namespace UsbPcapLib
{
  public class ThreadData
  {
    public string device;
    public USBPCAP_ADDRESS_FILTER filter;
    public EventWaitHandle ExitEvent;
    public IntPtr read_handle;
    public uint snaplen = SafeMethods.DEFAULT_SNAPSHOT_LENGTH;
    public uint bufferlen = SafeMethods.DEFAULT_INTERNAL_KERNEL_BUFFER_SIZE;
    public bool process = true;
    public bool pcapHeaderReadEver;

    public ThreadData(string device)
    {
        this.device = device;
    }
  }
}
