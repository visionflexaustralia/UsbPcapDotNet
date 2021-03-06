namespace UsbPcapDotNet;

public enum CONFIGRET
{
    CR_SUCCESS = 0,
    CR_DEFAULT = 1,
    CR_OUT_OF_MEMORY = 2,
    CR_INVALID_POINTER = 3,
    CR_INVALID_FLAG = 4,
    CR_INVALID_DEVINST = 5,
    CR_INVALID_DEVNODE = 5,
    CR_INVALID_RES_DES = 6,
    CR_INVALID_LOG_CONF = 7,
    CR_INVALID_ARBITRATOR = 8,
    CR_INVALID_NODELIST = 9,
    CR_DEVINST_HAS_REQS = 10, // 0x0000000A
    CR_DEVNODE_HAS_REQS = 10, // 0x0000000A
    CR_INVALID_RESOURCEID = 11, // 0x0000000B
    CR_DLVXD_NOT_FOUND = 12, // 0x0000000C
    CR_NO_SUCH_DEVINST = 13, // 0x0000000D
    CR_NO_SUCH_DEVNODE = 13, // 0x0000000D
    CR_NO_MORE_LOG_CONF = 14, // 0x0000000E
    CR_NO_MORE_RES_DES = 15, // 0x0000000F
    CR_ALREADY_SUCH_DEVINST = 16, // 0x00000010
    CR_ALREADY_SUCH_DEVNODE = 16, // 0x00000010
    CR_INVALID_RANGE_LIST = 17, // 0x00000011
    CR_INVALID_RANGE = 18, // 0x00000012
    CR_FAILURE = 19, // 0x00000013
    CR_NO_SUCH_LOGICAL_DEV = 20, // 0x00000014
    CR_CREATE_BLOCKED = 21, // 0x00000015
    CR_NOT_SYSTEM_VM = 22, // 0x00000016
    CR_REMOVE_VETOED = 23, // 0x00000017
    CR_APM_VETOED = 24, // 0x00000018
    CR_INVALID_LOAD_TYPE = 25, // 0x00000019
    CR_BUFFER_SMALL = 26, // 0x0000001A
    CR_NO_ARBITRATOR = 27, // 0x0000001B
    CR_NO_REGISTRY_HANDLE = 28, // 0x0000001C
    CR_REGISTRY_ERROR = 29, // 0x0000001D
    CR_INVALID_DEVICE_ID = 30, // 0x0000001E
    CR_INVALID_DATA = 31, // 0x0000001F
    CR_INVALID_API = 32, // 0x00000020
    CR_DEVLOADER_NOT_READY = 33, // 0x00000021
    CR_NEED_RESTART = 34, // 0x00000022
    CR_NO_MORE_HW_PROFILES = 35, // 0x00000023
    CR_DEVICE_NOT_THERE = 36, // 0x00000024
    CR_NO_SUCH_VALUE = 37, // 0x00000025
    CR_WRONG_TYPE = 38, // 0x00000026
    CR_INVALID_PRIORITY = 39, // 0x00000027
    CR_NOT_DISABLEABLE = 40, // 0x00000028
    CR_FREE_RESOURCES = 41, // 0x00000029
    CR_QUERY_VETOED = 42, // 0x0000002A
    CR_CANT_SHARE_IRQ = 43, // 0x0000002B
    CR_NO_DEPENDENT = 44, // 0x0000002C
    CR_SAME_RESOURCES = 45, // 0x0000002D
    CR_NO_SUCH_REGISTRY_KEY = 46, // 0x0000002E
    CR_INVALID_MACHINENAME = 47, // 0x0000002F
    CR_REMOTE_COMM_FAILURE = 48, // 0x00000030
    CR_MACHINE_UNAVAILABLE = 49, // 0x00000031
    CR_NO_CM_SERVICES = 50, // 0x00000032
    CR_ACCESS_DENIED = 51, // 0x00000033
    CR_CALL_NOT_IMPLEMENTED = 52, // 0x00000034
    CR_INVALID_PROPERTY = 53, // 0x00000035
    CR_DEVICE_INTERFACE_ACTIVE = 54, // 0x00000036
    CR_NO_SUCH_DEVICE_INTERFACE = 55, // 0x00000037
    CR_INVALID_REFERENCE_STRING = 56, // 0x00000038
    CR_INVALID_CONFLICT_LIST = 57, // 0x00000039
    CR_INVALID_INDEX = 58, // 0x0000003A
    CR_INVALID_STRUCTURE_SIZE = 59, // 0x0000003B
    NUM_CR_RESULTS = 60 // 0x0000003C
}
