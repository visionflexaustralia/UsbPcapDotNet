﻿namespace UsbPcapDotNet;

public enum URB_FUNCTION : ushort
{
    URB_FUNCTION_SELECT_CONFIGURATION,
    URB_FUNCTION_SELECT_INTERFACE,
    URB_FUNCTION_ABORT_PIPE,
    URB_FUNCTION_TAKE_FRAME_LENGTH_CONTROL,
    URB_FUNCTION_RELEASE_FRAME_LENGTH_CONTROL,
    URB_FUNCTION_GET_FRAME_LENGTH,
    URB_FUNCTION_SET_FRAME_LENGTH,
    URB_FUNCTION_GET_CURRENT_FRAME_NUMBER,
    URB_FUNCTION_CONTROL_TRANSFER,
    URB_FUNCTION_BULK_OR_INTERRUPT_TRANSFER,
    URB_FUNCTION_ISOCH_TRANSFER,
    URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE, // 0x000b
    URB_FUNCTION_SET_DESCRIPTOR_TO_DEVICE,
    URB_FUNCTION_SET_FEATURE_TO_DEVICE,
    URB_FUNCTION_SET_FEATURE_TO_INTERFACE,
    URB_FUNCTION_SET_FEATURE_TO_ENDPOINT,
    URB_FUNCTION_CLEAR_FEATURE_TO_DEVICE,
    URB_FUNCTION_CLEAR_FEATURE_TO_INTERFACE,
    URB_FUNCTION_CLEAR_FEATURE_TO_ENDPOINT,
    URB_FUNCTION_GET_STATUS_FROM_DEVICE,
    URB_FUNCTION_GET_STATUS_FROM_INTERFACE,
    URB_FUNCTION_GET_STATUS_FROM_ENDPOINT,
    URB_FUNCTION_RESERVED_0X0016,
    URB_FUNCTION_VENDOR_DEVICE,
    URB_FUNCTION_VENDOR_INTERFACE,
    URB_FUNCTION_VENDOR_ENDPOINT,
    URB_FUNCTION_CLASS_DEVICE,
    URB_FUNCTION_CLASS_INTERFACE,
    URB_FUNCTION_CLASS_ENDPOINT,
    URB_FUNCTION_RESERVE_0X001D,
    URB_FUNCTION_SYNC_RESET_PIPE_AND_CLEAR_STALL,
    URB_FUNCTION_CLASS_OTHER,
    URB_FUNCTION_VENDOR_OTHER,
    URB_FUNCTION_GET_STATUS_FROM_OTHER,
    URB_FUNCTION_CLEAR_FEATURE_TO_OTHER,
    URB_FUNCTION_SET_FEATURE_TO_OTHER,
    URB_FUNCTION_GET_DESCRIPTOR_FROM_ENDPOINT,
    URB_FUNCTION_SET_DESCRIPTOR_TO_ENDPOINT,
    URB_FUNCTION_GET_CONFIGURATION,
    URB_FUNCTION_GET_INTERFACE,
    URB_FUNCTION_GET_DESCRIPTOR_FROM_INTERFACE,
    URB_FUNCTION_SET_DESCRIPTOR_TO_INTERFACE,
    URB_FUNCTION_GET_MS_FEATURE_DESCRIPTOR,
    URB_FUNCTION_RESERVE_0X002B,
    URB_FUNCTION_RESERVE_0X002C,
    URB_FUNCTION_RESERVE_0X002D,
    URB_FUNCTION_RESERVE_0X002E,
    URB_FUNCTION_RESERVE_0X002F,
    URB_FUNCTION_SYNC_RESET_PIPE,
    URB_FUNCTION_SYNC_CLEAR_STALL,
    URB_FUNCTION_CONTROL_TRANSFER_EX,
    URB_FUNCTION_RESERVE_0X0033,
    URB_FUNCTION_RESERVE_0X0034,
    URB_FUNCTION_OPEN_STATIC_STREAMS,
    URB_FUNCTION_CLOSE_STATIC_STREAMS,
    URB_FUNCTION_BULK_OR_INTERRUPT_TRANSFER_USING_CHAINED_MDL,
    URB_FUNCTION_ISOCH_TRANSFER_USING_CHAINED_MDL
}
