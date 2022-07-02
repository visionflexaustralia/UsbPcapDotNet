namespace UsbPcapDotNet;

public unsafe struct list_entry
{
    public IntPtr data;
    public int length;
    public list_entry* next;
}

public unsafe struct port_descriptor_callback_context
{
    public ushort roothub;
    public USBPCAP_ADDRESS_FILTER addresses;
    public list_entry* head;
    public list_entry* tail;
}
