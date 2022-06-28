using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct OBJDIR_INFORMATION
{
    public UnicodeString ObjectName;
    public UnicodeString ObjectTypeName;
    public unsafe fixed byte Data[1];
}
