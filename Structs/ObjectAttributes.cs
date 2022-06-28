using System.Runtime.InteropServices;

namespace UsbPcapLib.Structs;

public struct ObjectAttributes : IDisposable
{
    public int Length;
    public IntPtr RootDirectory;
    private IntPtr objectName;
    public uint Attributes;
    public IntPtr SecurityDescriptor;
    public IntPtr SecurityQualityOfService;

    public ObjectAttributes(string name, uint attrs)
    {
        this.Length = 0;
        this.RootDirectory = IntPtr.Zero;
        this.objectName = IntPtr.Zero;
        this.Attributes = attrs;
        this.SecurityDescriptor = IntPtr.Zero;
        this.SecurityQualityOfService = IntPtr.Zero;
        this.Length = Marshal.SizeOf(this);
        this.ObjectName = new UnicodeString(name);
    }

    public UnicodeString ObjectName
    {
        get => (UnicodeString)Marshal.PtrToStructure(this.objectName, typeof(UnicodeString));
        set
        {
            var fDeleteOld = this.objectName != IntPtr.Zero;
            if (!fDeleteOld)
            {
                this.objectName = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            }

            Marshal.StructureToPtr(value, this.objectName, fDeleteOld);
        }
    }

    public void Dispose()
    {
        if (!(this.objectName != IntPtr.Zero))
        {
            return;
        }

        Marshal.DestroyStructure(this.objectName, typeof(UnicodeString));
        Marshal.FreeHGlobal(this.objectName);
        this.objectName = IntPtr.Zero;
    }
}
