using System.Runtime.InteropServices;

namespace UsbPcapDotNet;

public struct UnicodeString : IDisposable
{
    public ushort Length;
    public ushort MaximumLength;
    private IntPtr buffer;

    public UnicodeString(string s)
    {
        this.Length = (ushort)(s.Length * 2);
        this.MaximumLength = (ushort)(this.Length + 2U);
        this.buffer = Marshal.StringToHGlobalUni(s);
    }

    public void Dispose()
    {
        Marshal.FreeHGlobal(this.buffer);
        this.buffer = IntPtr.Zero;
    }

    public override string? ToString()
    {
        return Marshal.PtrToStringUni(this.buffer);
    }
}
