using System.Runtime.InteropServices;

namespace UsbPcapLib;

[StructLayout(LayoutKind.Sequential)]
public struct SP_PROPCHANGE_PARAMS
{
    public SP_CLASSINSTALL_HEADER ClassInstallHeader = new();
    public UInt32 StateChange;
    public UInt32 Scope;
    public UInt32 HwProfile;

    public SP_PROPCHANGE_PARAMS()
    {
        StateChange = 0;
        Scope = 0;
        HwProfile = 0;
    }
};
