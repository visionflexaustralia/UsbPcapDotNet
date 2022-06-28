using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

[StructLayout(LayoutKind.Sequential)]
public struct SP_PROPCHANGE_PARAMS
{
    public SP_CLASSINSTALL_HEADER ClassInstallHeader = new();
    public UInt32 StateChange;
    public UInt32 Scope;
    public UInt32 HwProfile;

    public SP_PROPCHANGE_PARAMS()
    {
        this.StateChange = 0;
        this.Scope = 0;
        this.HwProfile = 0;
    }
};
