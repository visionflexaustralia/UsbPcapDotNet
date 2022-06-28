namespace UsbPcapDotNet;

[Flags]
public enum SP_DEVINSTALL_PARAMS_FLAGS : int
{
    DI_NEEDRESTART               = 0x00000080,
    DI_NEEDREBOOT                = 0x00000100
}
