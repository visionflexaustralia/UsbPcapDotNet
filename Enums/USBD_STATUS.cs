﻿namespace UsbPcapLib.Enums
{
  public enum USBD_STATUS : uint
  {
    USBD_STATUS_SUCCESS = 0,
    USBD_STATUS_PORT_OPERATION_PENDING = 1,
    USBD_STATUS_PENDING = 1073741824, // 0x40000000
    USBD_STATUS_INVALID_URB_FUNCTION = 2147484160, // 0x80000200
    USBD_STATUS_INVALID_PARAMETER = 2147484416, // 0x80000300
    USBD_STATUS_ERROR_BUSY = 2147484672, // 0x80000400
    USBD_STATUS_INVALID_PIPE_HANDLE = 2147485184, // 0x80000600
    USBD_STATUS_NO_BANDWIDTH = 2147485440, // 0x80000700
    USBD_STATUS_INTERNAL_HC_ERROR = 2147485696, // 0x80000800
    USBD_STATUS_ERROR_SHORT_TRANSFER = 2147485952, // 0x80000900
    USBD_STATUS_CRC = 3221225473, // 0xC0000001
    USBD_STATUS_BTSTUFF = 3221225474, // 0xC0000002
    USBD_STATUS_DATA_TOGGLE_MISMATCH = 3221225475, // 0xC0000003
    USBD_STATUS_STALL_PID = 3221225476, // 0xC0000004
    USBD_STATUS_DEV_NOT_RESPONDING = 3221225477, // 0xC0000005
    USBD_STATUS_PID_CHECK_FAILURE = 3221225478, // 0xC0000006
    USBD_STATUS_UNEXPECTED_PID = 3221225479, // 0xC0000007
    USBD_STATUS_DATA_OVERRUN = 3221225480, // 0xC0000008
    USBD_STATUS_DATA_UNDERRUN = 3221225481, // 0xC0000009
    USBD_STATUS_RESERVED1 = 3221225482, // 0xC000000A
    USBD_STATUS_RESERVED2 = 3221225483, // 0xC000000B
    USBD_STATUS_BUFFER_OVERRUN = 3221225484, // 0xC000000C
    USBD_STATUS_BUFFER_UNDERRUN = 3221225485, // 0xC000000D
    USBD_STATUS_NOT_ACCESSED = 3221225487, // 0xC000000F
    USBD_STATUS_FIFO = 3221225488, // 0xC0000010
    USBD_STATUS_XACT_ERROR = 3221225489, // 0xC0000011
    USBD_STATUS_BABBLE_DETECTED = 3221225490, // 0xC0000012
    USBD_STATUS_DATA_BUFFER_ERROR = 3221225491, // 0xC0000013
    USBD_STATUS_NO_PING_RESPONSE = 3221225492, // 0xC0000014
    USBD_STATUS_INVALID_STREAM_TYPE = 3221225493, // 0xC0000015
    USBD_STATUS_INVALID_STREAM_ID = 3221225494, // 0xC0000016
    USBD_STATUS_ENDPOINT_HALTED = 3221225520, // 0xC0000030
    USBD_STATUS_BAD_START_FRAME = 3221228032, // 0xC0000A00
    USBD_STATUS_ISOCH_REQUEST_FAILED = 3221228288, // 0xC0000B00
    USBD_STATUS_FRAME_CONTROL_OWNED = 3221228544, // 0xC0000C00
    USBD_STATUS_FRAME_CONTROL_NOT_OWNED = 3221228800, // 0xC0000D00
    USBD_STATUS_NOT_SUPPORTED = 3221229056, // 0xC0000E00
    USBD_STATUS_INVALID_CONFIGURATION_DESCRIPTOR = 3221229312, // 0xC0000F00
    USBD_STATUS_INSUFFICIENT_RESOURCES = 3221229568, // 0xC0001000
    USBD_STATUS_SET_CONFIG_FAILED = 3221233664, // 0xC0002000
    USBD_STATUS_BUFFER_TOO_SMALL = 3221237760, // 0xC0003000
    USBD_STATUS_INTERFACE_NOT_FOUND = 3221241856, // 0xC0004000
    USBD_STATUS_INAVLID_PIPE_FLAGS = 3221245952, // 0xC0005000
    USBD_STATUS_TIMEOUT = 3221250048, // 0xC0006000
    USBD_STATUS_DEVICE_GONE = 3221254144, // 0xC0007000
    USBD_STATUS_STATUS_NOT_MAPPED = 3221258240, // 0xC0008000
    USBD_STATUS_HUB_INTERNAL_ERROR = 3221262336, // 0xC0009000
    USBD_STATUS_CANCELED = 3221291008, // 0xC0010000
    USBD_STATUS_ISO_NOT_ACCESSED_BY_HW = 3221356544, // 0xC0020000
    USBD_STATUS_ISO_TD_ERROR = 3221422080, // 0xC0030000
    USBD_STATUS_ISO_NA_LATE_USBPORT = 3221487616, // 0xC0040000
    USBD_STATUS_ISO_NOT_ACCESSED_LATE = 3221553152, // 0xC0050000
    USBD_STATUS_BAD_DESCRIPTOR = 3222274048, // 0xC0100000
    USBD_STATUS_BAD_DESCRIPTOR_BLEN = 3222274049, // 0xC0100001
    USBD_STATUS_BAD_DESCRIPTOR_TYPE = 3222274050, // 0xC0100002
    USBD_STATUS_BAD_INTERFACE_DESCRIPTOR = 3222274051, // 0xC0100003
    USBD_STATUS_BAD_ENDPOINT_DESCRIPTOR = 3222274052, // 0xC0100004
    USBD_STATUS_BAD_INTERFACE_ASSOC_DESCRIPTOR = 3222274053, // 0xC0100005
    USBD_STATUS_BAD_CONFIG_DESC_LENGTH = 3222274054, // 0xC0100006
    USBD_STATUS_BAD_NUMBER_OF_INTERFACES = 3222274055, // 0xC0100007
    USBD_STATUS_BAD_NUMBER_OF_ENDPOINTS = 3222274056, // 0xC0100008
    USBD_STATUS_BAD_ENDPOINT_ADDRESS = 3222274057, // 0xC0100009
  }
}
