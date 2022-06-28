namespace UsbPcapDotNet;

using sFileAccess = System.IO.FileAccess;

[Flags]
public enum EIOControlCode : uint
{
    // STORAGE
    StorageCheckVerify = (EFileDevice.MassStorage << 16) | (0x0200 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),

    StorageCheckVerify2 =
        (EFileDevice.MassStorage << 16) | (0x0200 << 2) | EMethod.Buffered | (0 << 14), // sFileAccess.Any
    StorageMediaRemoval = (EFileDevice.MassStorage << 16) | (0x0201 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    StorageEjectMedia = (EFileDevice.MassStorage << 16) | (0x0202 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    StorageLoadMedia = (EFileDevice.MassStorage << 16) | (0x0203 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    StorageLoadMedia2 = (EFileDevice.MassStorage << 16) | (0x0203 << 2) | EMethod.Buffered | (0 << 14),
    StorageReserve = (EFileDevice.MassStorage << 16) | (0x0204 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    StorageRelease = (EFileDevice.MassStorage << 16) | (0x0205 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),

    StorageFindNewDevices =
        (EFileDevice.MassStorage << 16) | (0x0206 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    StorageEjectionControl = (EFileDevice.MassStorage << 16) | (0x0250 << 2) | EMethod.Buffered | (0 << 14),
    StorageMcnControl = (EFileDevice.MassStorage << 16) | (0x0251 << 2) | EMethod.Buffered | (0 << 14),
    StorageGetMediaTypes = (EFileDevice.MassStorage << 16) | (0x0300 << 2) | EMethod.Buffered | (0 << 14),
    StorageGetMediaTypesEx = (EFileDevice.MassStorage << 16) | (0x0301 << 2) | EMethod.Buffered | (0 << 14),
    StorageResetBus = (EFileDevice.MassStorage << 16) | (0x0400 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    StorageResetDevice = (EFileDevice.MassStorage << 16) | (0x0401 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    StorageGetDeviceNumber = (EFileDevice.MassStorage << 16) | (0x0420 << 2) | EMethod.Buffered | (0 << 14),
    StoragePredictFailure = (EFileDevice.MassStorage << 16) | (0x0440 << 2) | EMethod.Buffered | (0 << 14),

    StorageObsoleteResetBus = (EFileDevice.MassStorage << 16) | (0x0400 << 2) | EMethod.Buffered
                            | (sFileAccess.ReadWrite << 14),

    StorageObsoleteResetDevice = (EFileDevice.MassStorage << 16) | (0x0401 << 2) | EMethod.Buffered
                               | (sFileAccess.ReadWrite << 14),
    StorageQueryProperty = (EFileDevice.MassStorage << 16) | (0x0500 << 2) | EMethod.Buffered | (0 << 14),

    // DISK
    DiskGetDriveGeometry = (EFileDevice.Disk << 16) | (0x0000 << 2) | EMethod.Buffered | (0 << 14),
    DiskGetDriveGeometryEx = (EFileDevice.Disk << 16) | (0x0028 << 2) | EMethod.Buffered | (0 << 14),
    DiskGetPartitionInfo = (EFileDevice.Disk << 16) | (0x0001 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    DiskGetPartitionInfoEx = (EFileDevice.Disk << 16) | (0x0012 << 2) | EMethod.Buffered | (0 << 14),
    DiskSetPartitionInfo = (EFileDevice.Disk << 16) | (0x0002 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskGetDriveLayout = (EFileDevice.Disk << 16) | (0x0003 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    DiskSetDriveLayout = (EFileDevice.Disk << 16) | (0x0004 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskVerify = (EFileDevice.Disk << 16) | (0x0005 << 2) | EMethod.Buffered | (0 << 14),
    DiskFormatTracks = (EFileDevice.Disk << 16) | (0x0006 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskReassignBlocks = (EFileDevice.Disk << 16) | (0x0007 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskPerformance = (EFileDevice.Disk << 16) | (0x0008 << 2) | EMethod.Buffered | (0 << 14),
    DiskIsWritable = (EFileDevice.Disk << 16) | (0x0009 << 2) | EMethod.Buffered | (0 << 14),
    DiskLogging = (EFileDevice.Disk << 16) | (0x000a << 2) | EMethod.Buffered | (0 << 14),
    DiskFormatTracksEx = (EFileDevice.Disk << 16) | (0x000b << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskHistogramStructure = (EFileDevice.Disk << 16) | (0x000c << 2) | EMethod.Buffered | (0 << 14),
    DiskHistogramData = (EFileDevice.Disk << 16) | (0x000d << 2) | EMethod.Buffered | (0 << 14),
    DiskHistogramReset = (EFileDevice.Disk << 16) | (0x000e << 2) | EMethod.Buffered | (0 << 14),
    DiskRequestStructure = (EFileDevice.Disk << 16) | (0x000f << 2) | EMethod.Buffered | (0 << 14),
    DiskRequestData = (EFileDevice.Disk << 16) | (0x0010 << 2) | EMethod.Buffered | (0 << 14),
    DiskControllerNumber = (EFileDevice.Disk << 16) | (0x0011 << 2) | EMethod.Buffered | (0 << 14),
    DiskSmartGetVersion = (EFileDevice.Disk << 16) | (0x0020 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),

    DiskSmartSendDriveCommand =
        (EFileDevice.Disk << 16) | (0x0021 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskSmartRcvDriveData = (EFileDevice.Disk << 16) | (0x0022 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskUpdateDriveSize = (EFileDevice.Disk << 16) | (0x0032 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskGrowPartition = (EFileDevice.Disk << 16) | (0x0034 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskGetCacheInformation = (EFileDevice.Disk << 16) | (0x0035 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),

    DiskSetCacheInformation =
        (EFileDevice.Disk << 16) | (0x0036 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskDeleteDriveLayout = (EFileDevice.Disk << 16) | (0x0040 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskFormatDrive = (EFileDevice.Disk << 16) | (0x00f3 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskSenseDevice = (EFileDevice.Disk << 16) | (0x00f8 << 2) | EMethod.Buffered | (0 << 14),
    DiskCheckVerify = (EFileDevice.Disk << 16) | (0x0200 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    DiskMediaRemoval = (EFileDevice.Disk << 16) | (0x0201 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    DiskEjectMedia = (EFileDevice.Disk << 16) | (0x0202 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    DiskLoadMedia = (EFileDevice.Disk << 16) | (0x0203 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    DiskReserve = (EFileDevice.Disk << 16) | (0x0204 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    DiskRelease = (EFileDevice.Disk << 16) | (0x0205 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    DiskFindNewDevices = (EFileDevice.Disk << 16) | (0x0206 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    DiskGetMediaTypes = (EFileDevice.Disk << 16) | (0x0300 << 2) | EMethod.Buffered | (0 << 14),

    DiskSetPartitionInfoEx =
        (EFileDevice.Disk << 16) | (0x0013 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskGetDriveLayoutEx = (EFileDevice.Disk << 16) | (0x0014 << 2) | EMethod.Buffered | (0 << 14),
    DiskSetDriveLayoutEx = (EFileDevice.Disk << 16) | (0x0015 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskCreateDisk = (EFileDevice.Disk << 16) | (0x0016 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    DiskGetLengthInfo = (EFileDevice.Disk << 16) | (0x0017 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),

    // CHANGER
    ChangerGetParameters = (EFileDevice.Changer << 16) | (0x0000 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    ChangerGetStatus = (EFileDevice.Changer << 16) | (0x0001 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    ChangerGetProductData = (EFileDevice.Changer << 16) | (0x0002 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    ChangerSetAccess = (EFileDevice.Changer << 16) | (0x0004 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),

    ChangerGetElementStatus =
        (EFileDevice.Changer << 16) | (0x0005 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),

    ChangerInitializeElementStatus =
        (EFileDevice.Changer << 16) | (0x0006 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    ChangerSetPosition = (EFileDevice.Changer << 16) | (0x0007 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    ChangerExchangeMedium = (EFileDevice.Changer << 16) | (0x0008 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),
    ChangerMoveMedium = (EFileDevice.Changer << 16) | (0x0009 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),

    ChangerReinitializeTarget =
        (EFileDevice.Changer << 16) | (0x000A << 2) | EMethod.Buffered | (sFileAccess.Read << 14),

    ChangerQueryVolumeTags =
        (EFileDevice.Changer << 16) | (0x000B << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),

    // FILESYSTEM
    FsctlRequestOplockLevel1 = (EFileDevice.FileSystem << 16) | (0 << 2) | EMethod.Buffered | (0 << 14),
    FsctlRequestOplockLevel2 = (EFileDevice.FileSystem << 16) | (1 << 2) | EMethod.Buffered | (0 << 14),
    FsctlRequestBatchOplock = (EFileDevice.FileSystem << 16) | (2 << 2) | EMethod.Buffered | (0 << 14),
    FsctlOplockBreakAcknowledge = (EFileDevice.FileSystem << 16) | (3 << 2) | EMethod.Buffered | (0 << 14),
    FsctlOpBatchAckClosePending = (EFileDevice.FileSystem << 16) | (4 << 2) | EMethod.Buffered | (0 << 14),
    FsctlOplockBreakNotify = (EFileDevice.FileSystem << 16) | (5 << 2) | EMethod.Buffered | (0 << 14),
    FsctlLockVolume = (EFileDevice.FileSystem << 16) | (6 << 2) | EMethod.Buffered | (0 << 14),
    FsctlUnlockVolume = (EFileDevice.FileSystem << 16) | (7 << 2) | EMethod.Buffered | (0 << 14),
    FsctlDismountVolume = (EFileDevice.FileSystem << 16) | (8 << 2) | EMethod.Buffered | (0 << 14),
    FsctlIsVolumeMounted = (EFileDevice.FileSystem << 16) | (10 << 2) | EMethod.Buffered | (0 << 14),
    FsctlIsPathnameValid = (EFileDevice.FileSystem << 16) | (11 << 2) | EMethod.Buffered | (0 << 14),
    FsctlMarkVolumeDirty = (EFileDevice.FileSystem << 16) | (12 << 2) | EMethod.Buffered | (0 << 14),
    FsctlQueryRetrievalPointers = (EFileDevice.FileSystem << 16) | (14 << 2) | EMethod.Neither | (0 << 14),
    FsctlGetCompression = (EFileDevice.FileSystem << 16) | (15 << 2) | EMethod.Buffered | (0 << 14),
    FsctlSetCompression = (EFileDevice.FileSystem << 16) | (16 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    FsctlMarkAsSystemHive = (EFileDevice.FileSystem << 16) | (19 << 2) | EMethod.Neither | (0 << 14),
    FsctlOplockBreakAckNo2 = (EFileDevice.FileSystem << 16) | (20 << 2) | EMethod.Buffered | (0 << 14),
    FsctlInvalidateVolumes = (EFileDevice.FileSystem << 16) | (21 << 2) | EMethod.Buffered | (0 << 14),
    FsctlQueryFatBpb = (EFileDevice.FileSystem << 16) | (22 << 2) | EMethod.Buffered | (0 << 14),
    FsctlRequestFilterOplock = (EFileDevice.FileSystem << 16) | (23 << 2) | EMethod.Buffered | (0 << 14),
    FsctlFileSystemGetStatistics = (EFileDevice.FileSystem << 16) | (24 << 2) | EMethod.Buffered | (0 << 14),
    FsctlGetNtfsVolumeData = (EFileDevice.FileSystem << 16) | (25 << 2) | EMethod.Buffered | (0 << 14),
    FsctlGetNtfsFileRecord = (EFileDevice.FileSystem << 16) | (26 << 2) | EMethod.Buffered | (0 << 14),
    FsctlGetVolumeBitmap = (EFileDevice.FileSystem << 16) | (27 << 2) | EMethod.Neither | (0 << 14),
    FsctlGetRetrievalPointers = (EFileDevice.FileSystem << 16) | (28 << 2) | EMethod.Neither | (0 << 14),
    FsctlMoveFile = (EFileDevice.FileSystem << 16) | (29 << 2) | EMethod.Buffered | (0 << 14),
    FsctlIsVolumeDirty = (EFileDevice.FileSystem << 16) | (30 << 2) | EMethod.Buffered | (0 << 14),
    FsctlGetHfsInformation = (EFileDevice.FileSystem << 16) | (31 << 2) | EMethod.Buffered | (0 << 14),
    FsctlAllowExtendedDasdIo = (EFileDevice.FileSystem << 16) | (32 << 2) | EMethod.Neither | (0 << 14),
    FsctlReadPropertyData = (EFileDevice.FileSystem << 16) | (33 << 2) | EMethod.Neither | (0 << 14),
    FsctlWritePropertyData = (EFileDevice.FileSystem << 16) | (34 << 2) | EMethod.Neither | (0 << 14),
    FsctlFindFilesBySid = (EFileDevice.FileSystem << 16) | (35 << 2) | EMethod.Neither | (0 << 14),
    FsctlDumpPropertyData = (EFileDevice.FileSystem << 16) | (37 << 2) | EMethod.Neither | (0 << 14),
    FsctlSetObjectId = (EFileDevice.FileSystem << 16) | (38 << 2) | EMethod.Buffered | (0 << 14),
    FsctlGetObjectId = (EFileDevice.FileSystem << 16) | (39 << 2) | EMethod.Buffered | (0 << 14),
    FsctlDeleteObjectId = (EFileDevice.FileSystem << 16) | (40 << 2) | EMethod.Buffered | (0 << 14),
    FsctlSetReparsePoint = (EFileDevice.FileSystem << 16) | (41 << 2) | EMethod.Buffered | (0 << 14),
    FsctlGetReparsePoint = (EFileDevice.FileSystem << 16) | (42 << 2) | EMethod.Buffered | (0 << 14),
    FsctlDeleteReparsePoint = (EFileDevice.FileSystem << 16) | (43 << 2) | EMethod.Buffered | (0 << 14),
    FsctlEnumUsnData = (EFileDevice.FileSystem << 16) | (44 << 2) | EMethod.Neither | (0 << 14),
    FsctlSecurityIdCheck = (EFileDevice.FileSystem << 16) | (45 << 2) | EMethod.Neither | (sFileAccess.Read << 14),
    FsctlReadUsnJournal = (EFileDevice.FileSystem << 16) | (46 << 2) | EMethod.Neither | (0 << 14),
    FsctlSetObjectIdExtended = (EFileDevice.FileSystem << 16) | (47 << 2) | EMethod.Buffered | (0 << 14),
    FsctlCreateOrGetObjectId = (EFileDevice.FileSystem << 16) | (48 << 2) | EMethod.Buffered | (0 << 14),
    FsctlSetSparse = (EFileDevice.FileSystem << 16) | (49 << 2) | EMethod.Buffered | (0 << 14),
    FsctlSetZeroData = (EFileDevice.FileSystem << 16) | (50 << 2) | EMethod.Buffered | (sFileAccess.Write << 14),
    FsctlQueryAllocatedRanges = (EFileDevice.FileSystem << 16) | (51 << 2) | EMethod.Neither | (sFileAccess.Read << 14),
    FsctlEnableUpgrade = (EFileDevice.FileSystem << 16) | (52 << 2) | EMethod.Buffered | (sFileAccess.Write << 14),
    FsctlSetEncryption = (EFileDevice.FileSystem << 16) | (53 << 2) | EMethod.Neither | (0 << 14),
    FsctlEncryptionFsctlIo = (EFileDevice.FileSystem << 16) | (54 << 2) | EMethod.Neither | (0 << 14),
    FsctlWriteRawEncrypted = (EFileDevice.FileSystem << 16) | (55 << 2) | EMethod.Neither | (0 << 14),
    FsctlReadRawEncrypted = (EFileDevice.FileSystem << 16) | (56 << 2) | EMethod.Neither | (0 << 14),
    FsctlCreateUsnJournal = (EFileDevice.FileSystem << 16) | (57 << 2) | EMethod.Neither | (0 << 14),
    FsctlReadFileUsnData = (EFileDevice.FileSystem << 16) | (58 << 2) | EMethod.Neither | (0 << 14),
    FsctlWriteUsnCloseRecord = (EFileDevice.FileSystem << 16) | (59 << 2) | EMethod.Neither | (0 << 14),
    FsctlExtendVolume = (EFileDevice.FileSystem << 16) | (60 << 2) | EMethod.Buffered | (0 << 14),
    FsctlQueryUsnJournal = (EFileDevice.FileSystem << 16) | (61 << 2) | EMethod.Buffered | (0 << 14),
    FsctlDeleteUsnJournal = (EFileDevice.FileSystem << 16) | (62 << 2) | EMethod.Buffered | (0 << 14),
    FsctlMarkHandle = (EFileDevice.FileSystem << 16) | (63 << 2) | EMethod.Buffered | (0 << 14),
    FsctlSisCopyFile = (EFileDevice.FileSystem << 16) | (64 << 2) | EMethod.Buffered | (0 << 14),
    FsctlSisLinkFiles = (EFileDevice.FileSystem << 16) | (65 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    FsctlHsmMsg = (EFileDevice.FileSystem << 16) | (66 << 2) | EMethod.Buffered | (sFileAccess.ReadWrite << 14),
    FsctlNssControl = (EFileDevice.FileSystem << 16) | (67 << 2) | EMethod.Buffered | (sFileAccess.Write << 14),
    FsctlHsmData = (EFileDevice.FileSystem << 16) | (68 << 2) | EMethod.Neither | (sFileAccess.ReadWrite << 14),
    FsctlRecallFile = (EFileDevice.FileSystem << 16) | (69 << 2) | EMethod.Neither | (0 << 14),
    FsctlNssRcontrol = (EFileDevice.FileSystem << 16) | (70 << 2) | EMethod.Buffered | (sFileAccess.Read << 14),

    // VIDEO
    VideoQuerySupportedBrightness = (EFileDevice.Video << 16) | (0x0125 << 2) | EMethod.Buffered | (0 << 14),
    VideoQueryDisplayBrightness = (EFileDevice.Video << 16) | (0x0126 << 2) | EMethod.Buffered | (0 << 14),
    VideoSetDisplayBrightness = (EFileDevice.Video << 16) | (0x0127 << 2) | EMethod.Buffered | (0 << 14)
}
