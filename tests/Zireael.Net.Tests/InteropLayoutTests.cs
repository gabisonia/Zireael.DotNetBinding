using System.Runtime.InteropServices;

namespace Zireael.Net.Tests;

public class InteropLayoutTests
{
    [Fact]
    public void PlatformAndConfigStructs_ShouldMatchExpectedSizes()
    {
        // Arrange
        var expected = new Dictionary<string, int>
        {
            [nameof(ZrPlatformSize)] = 8,
            [nameof(ZrPlatformCaps)] = 16,
            [nameof(ZrPlatformConfig)] = 8,
            [nameof(ZrLimits)] = 40,
            [nameof(ZrEngineConfig)] = 92,
            [nameof(ZrEngineRuntimeConfig)] = 72
        };

        // Act
        var actual = new Dictionary<string, int>
        {
            [nameof(ZrPlatformSize)] = GetSize<ZrPlatformSize>(),
            [nameof(ZrPlatformCaps)] = GetSize<ZrPlatformCaps>(),
            [nameof(ZrPlatformConfig)] = GetSize<ZrPlatformConfig>(),
            [nameof(ZrLimits)] = GetSize<ZrLimits>(),
            [nameof(ZrEngineConfig)] = GetSize<ZrEngineConfig>(),
            [nameof(ZrEngineRuntimeConfig)] = GetSize<ZrEngineRuntimeConfig>()
        };

        // Assert
        AssertIntMap(expected, actual);
    }

    [Fact]
    public void PlatformAndConfigStructs_ShouldMatchExpectedOffsets()
    {
        // Arrange
        var expected = new Dictionary<string, int>
        {
            [$"{nameof(ZrEngineConfig)}.{nameof(ZrEngineConfig.Limits)}"] = 20,
            [$"{nameof(ZrEngineConfig)}.{nameof(ZrEngineConfig.Platform)}"] = 60,
            [$"{nameof(ZrEngineConfig)}.{nameof(ZrEngineConfig.TabWidth)}"] = 68,
            [$"{nameof(ZrEngineConfig)}.{nameof(ZrEngineConfig.EnableScrollOptimizations)}"] = 80,
            [$"{nameof(ZrEngineConfig)}.{nameof(ZrEngineConfig.CapForceFlags)}"] = 84,
            [$"{nameof(ZrEngineConfig)}.{nameof(ZrEngineConfig.CapSuppressFlags)}"] = 88,
            [$"{nameof(ZrEngineRuntimeConfig)}.{nameof(ZrEngineRuntimeConfig.Limits)}"] = 0,
            [$"{nameof(ZrEngineRuntimeConfig)}.{nameof(ZrEngineRuntimeConfig.Platform)}"] = 40,
            [$"{nameof(ZrEngineRuntimeConfig)}.{nameof(ZrEngineRuntimeConfig.TabWidth)}"] = 48,
            [$"{nameof(ZrEngineRuntimeConfig)}.{nameof(ZrEngineRuntimeConfig.EnableScrollOptimizations)}"] = 60,
            [$"{nameof(ZrEngineRuntimeConfig)}.{nameof(ZrEngineRuntimeConfig.CapForceFlags)}"] = 64,
            [$"{nameof(ZrEngineRuntimeConfig)}.{nameof(ZrEngineRuntimeConfig.CapSuppressFlags)}"] = 68
        };

        // Act
        var actual = new Dictionary<string, int>
        {
            [$"{nameof(ZrEngineConfig)}.{nameof(ZrEngineConfig.Limits)}"] =
                GetOffset<ZrEngineConfig>(nameof(ZrEngineConfig.Limits)),
            [$"{nameof(ZrEngineConfig)}.{nameof(ZrEngineConfig.Platform)}"] =
                GetOffset<ZrEngineConfig>(nameof(ZrEngineConfig.Platform)),
            [$"{nameof(ZrEngineConfig)}.{nameof(ZrEngineConfig.TabWidth)}"] =
                GetOffset<ZrEngineConfig>(nameof(ZrEngineConfig.TabWidth)),
            [$"{nameof(ZrEngineConfig)}.{nameof(ZrEngineConfig.EnableScrollOptimizations)}"] =
                GetOffset<ZrEngineConfig>(nameof(ZrEngineConfig.EnableScrollOptimizations)),
            [$"{nameof(ZrEngineConfig)}.{nameof(ZrEngineConfig.CapForceFlags)}"] =
                GetOffset<ZrEngineConfig>(nameof(ZrEngineConfig.CapForceFlags)),
            [$"{nameof(ZrEngineConfig)}.{nameof(ZrEngineConfig.CapSuppressFlags)}"] =
                GetOffset<ZrEngineConfig>(nameof(ZrEngineConfig.CapSuppressFlags)),
            [$"{nameof(ZrEngineRuntimeConfig)}.{nameof(ZrEngineRuntimeConfig.Limits)}"] =
                GetOffset<ZrEngineRuntimeConfig>(nameof(ZrEngineRuntimeConfig.Limits)),
            [$"{nameof(ZrEngineRuntimeConfig)}.{nameof(ZrEngineRuntimeConfig.Platform)}"] =
                GetOffset<ZrEngineRuntimeConfig>(nameof(ZrEngineRuntimeConfig.Platform)),
            [$"{nameof(ZrEngineRuntimeConfig)}.{nameof(ZrEngineRuntimeConfig.TabWidth)}"] =
                GetOffset<ZrEngineRuntimeConfig>(nameof(ZrEngineRuntimeConfig.TabWidth)),
            [$"{nameof(ZrEngineRuntimeConfig)}.{nameof(ZrEngineRuntimeConfig.EnableScrollOptimizations)}"] =
                GetOffset<ZrEngineRuntimeConfig>(nameof(ZrEngineRuntimeConfig.EnableScrollOptimizations)),
            [$"{nameof(ZrEngineRuntimeConfig)}.{nameof(ZrEngineRuntimeConfig.CapForceFlags)}"] =
                GetOffset<ZrEngineRuntimeConfig>(nameof(ZrEngineRuntimeConfig.CapForceFlags)),
            [$"{nameof(ZrEngineRuntimeConfig)}.{nameof(ZrEngineRuntimeConfig.CapSuppressFlags)}"] =
                GetOffset<ZrEngineRuntimeConfig>(nameof(ZrEngineRuntimeConfig.CapSuppressFlags))
        };

        // Assert
        AssertIntMap(expected, actual);
    }

    [Fact]
    public void MetricsStruct_ShouldMatchExpectedLayout()
    {
        // Arrange
        var expected = new Dictionary<string, int>
        {
            [nameof(ZrMetrics)] = 120,
            [$"{nameof(ZrMetrics)}.{nameof(ZrMetrics.StructSize)}"] = 0,
            [$"{nameof(ZrMetrics)}.{nameof(ZrMetrics.FrameIndex)}"] = 24,
            [$"{nameof(ZrMetrics)}.{nameof(ZrMetrics.BytesEmittedTotal)}"] = 40,
            [$"{nameof(ZrMetrics)}.{nameof(ZrMetrics.ArenaFrameHighWaterBytes)}"] = 88,
            [$"{nameof(ZrMetrics)}.{nameof(ZrMetrics.DamageFullFrame)}"] = 112
        };

        // Act
        var actual = new Dictionary<string, int>
        {
            [nameof(ZrMetrics)] = GetSize<ZrMetrics>(),
            [$"{nameof(ZrMetrics)}.{nameof(ZrMetrics.StructSize)}"] =
                GetOffset<ZrMetrics>(nameof(ZrMetrics.StructSize)),
            [$"{nameof(ZrMetrics)}.{nameof(ZrMetrics.FrameIndex)}"] =
                GetOffset<ZrMetrics>(nameof(ZrMetrics.FrameIndex)),
            [$"{nameof(ZrMetrics)}.{nameof(ZrMetrics.BytesEmittedTotal)}"] =
                GetOffset<ZrMetrics>(nameof(ZrMetrics.BytesEmittedTotal)),
            [$"{nameof(ZrMetrics)}.{nameof(ZrMetrics.ArenaFrameHighWaterBytes)}"] =
                GetOffset<ZrMetrics>(nameof(ZrMetrics.ArenaFrameHighWaterBytes)),
            [$"{nameof(ZrMetrics)}.{nameof(ZrMetrics.DamageFullFrame)}"] =
                GetOffset<ZrMetrics>(nameof(ZrMetrics.DamageFullFrame))
        };

        // Assert
        AssertIntMap(expected, actual);
    }

    [Fact]
    public void EventStructs_ShouldMatchExpectedSizes()
    {
        // Arrange
        var expected = new Dictionary<string, int>
        {
            [nameof(ZrEvBatchHeader)] = 24,
            [nameof(ZrEvRecordHeader)] = 16,
            [nameof(ZrEvKey)] = 16,
            [nameof(ZrEvText)] = 8,
            [nameof(ZrEvPaste)] = 8,
            [nameof(ZrEvMouse)] = 32,
            [nameof(ZrEvResize)] = 16,
            [nameof(ZrEvTick)] = 16,
            [nameof(ZrEvUser)] = 16
        };

        // Act
        var actual = new Dictionary<string, int>
        {
            [nameof(ZrEvBatchHeader)] = GetSize<ZrEvBatchHeader>(),
            [nameof(ZrEvRecordHeader)] = GetSize<ZrEvRecordHeader>(),
            [nameof(ZrEvKey)] = GetSize<ZrEvKey>(),
            [nameof(ZrEvText)] = GetSize<ZrEvText>(),
            [nameof(ZrEvPaste)] = GetSize<ZrEvPaste>(),
            [nameof(ZrEvMouse)] = GetSize<ZrEvMouse>(),
            [nameof(ZrEvResize)] = GetSize<ZrEvResize>(),
            [nameof(ZrEvTick)] = GetSize<ZrEvTick>(),
            [nameof(ZrEvUser)] = GetSize<ZrEvUser>()
        };

        // Assert
        AssertIntMap(expected, actual);
    }

    [Fact]
    public void DrawlistStructs_ShouldMatchExpectedSizes()
    {
        // Arrange
        var expected = new Dictionary<string, int>
        {
            [nameof(ZrDlHeader)] = 64,
            [nameof(ZrDlSpan)] = 8,
            [nameof(ZrDlCmdHeader)] = 8,
            [nameof(ZrDlStyle)] = 16,
            [nameof(ZrDlStyleV3Ext)] = 12,
            [nameof(ZrDlStyleV3)] = 28,
            [nameof(ZrDlCmdFillRect)] = 32,
            [nameof(ZrDlCmdDrawText)] = 40,
            [nameof(ZrDlCmdFillRectV3)] = 44,
            [nameof(ZrDlCmdDrawTextV3)] = 52,
            [nameof(ZrDlCmdPushClip)] = 16,
            [nameof(ZrDlCmdDrawTextRun)] = 16,
            [nameof(ZrDlTextRunSegmentV3)] = 40,
            [nameof(ZrDlCmdSetCursor)] = 12,
            [nameof(ZrDlCmdDrawCanvas)] = 24,
            [nameof(ZrDlCmdDrawImage)] = 32
        };

        // Act
        var actual = new Dictionary<string, int>
        {
            [nameof(ZrDlHeader)] = GetSize<ZrDlHeader>(),
            [nameof(ZrDlSpan)] = GetSize<ZrDlSpan>(),
            [nameof(ZrDlCmdHeader)] = GetSize<ZrDlCmdHeader>(),
            [nameof(ZrDlStyle)] = GetSize<ZrDlStyle>(),
            [nameof(ZrDlStyleV3Ext)] = GetSize<ZrDlStyleV3Ext>(),
            [nameof(ZrDlStyleV3)] = GetSize<ZrDlStyleV3>(),
            [nameof(ZrDlCmdFillRect)] = GetSize<ZrDlCmdFillRect>(),
            [nameof(ZrDlCmdDrawText)] = GetSize<ZrDlCmdDrawText>(),
            [nameof(ZrDlCmdFillRectV3)] = GetSize<ZrDlCmdFillRectV3>(),
            [nameof(ZrDlCmdDrawTextV3)] = GetSize<ZrDlCmdDrawTextV3>(),
            [nameof(ZrDlCmdPushClip)] = GetSize<ZrDlCmdPushClip>(),
            [nameof(ZrDlCmdDrawTextRun)] = GetSize<ZrDlCmdDrawTextRun>(),
            [nameof(ZrDlTextRunSegmentV3)] = GetSize<ZrDlTextRunSegmentV3>(),
            [nameof(ZrDlCmdSetCursor)] = GetSize<ZrDlCmdSetCursor>(),
            [nameof(ZrDlCmdDrawCanvas)] = GetSize<ZrDlCmdDrawCanvas>(),
            [nameof(ZrDlCmdDrawImage)] = GetSize<ZrDlCmdDrawImage>()
        };

        // Assert
        AssertIntMap(expected, actual);
    }

    [Fact]
    public void DrawlistCommandStructs_ShouldMatchExpectedOffsets()
    {
        // Arrange
        var expected = new Dictionary<string, int>
        {
            [$"{nameof(ZrDlCmdDrawCanvas)}.{nameof(ZrDlCmdDrawCanvas.BlobOffset)}"] = 12,
            [$"{nameof(ZrDlCmdDrawCanvas)}.{nameof(ZrDlCmdDrawCanvas.BlobLen)}"] = 16,
            [$"{nameof(ZrDlCmdDrawCanvas)}.{nameof(ZrDlCmdDrawCanvas.Blitter)}"] = 20,
            [$"{nameof(ZrDlCmdDrawImage)}.{nameof(ZrDlCmdDrawImage.BlobOffset)}"] = 12,
            [$"{nameof(ZrDlCmdDrawImage)}.{nameof(ZrDlCmdDrawImage.ImageId)}"] = 20,
            [$"{nameof(ZrDlCmdDrawImage)}.{nameof(ZrDlCmdDrawImage.Format)}"] = 24,
            [$"{nameof(ZrDlCmdDrawImage)}.{nameof(ZrDlCmdDrawImage.ZLayer)}"] = 26,
            [$"{nameof(ZrDlCmdDrawImage)}.{nameof(ZrDlCmdDrawImage.Reserved1)}"] = 30
        };

        // Act
        var actual = new Dictionary<string, int>
        {
            [$"{nameof(ZrDlCmdDrawCanvas)}.{nameof(ZrDlCmdDrawCanvas.BlobOffset)}"] =
                GetOffset<ZrDlCmdDrawCanvas>(nameof(ZrDlCmdDrawCanvas.BlobOffset)),
            [$"{nameof(ZrDlCmdDrawCanvas)}.{nameof(ZrDlCmdDrawCanvas.BlobLen)}"] =
                GetOffset<ZrDlCmdDrawCanvas>(nameof(ZrDlCmdDrawCanvas.BlobLen)),
            [$"{nameof(ZrDlCmdDrawCanvas)}.{nameof(ZrDlCmdDrawCanvas.Blitter)}"] =
                GetOffset<ZrDlCmdDrawCanvas>(nameof(ZrDlCmdDrawCanvas.Blitter)),
            [$"{nameof(ZrDlCmdDrawImage)}.{nameof(ZrDlCmdDrawImage.BlobOffset)}"] =
                GetOffset<ZrDlCmdDrawImage>(nameof(ZrDlCmdDrawImage.BlobOffset)),
            [$"{nameof(ZrDlCmdDrawImage)}.{nameof(ZrDlCmdDrawImage.ImageId)}"] =
                GetOffset<ZrDlCmdDrawImage>(nameof(ZrDlCmdDrawImage.ImageId)),
            [$"{nameof(ZrDlCmdDrawImage)}.{nameof(ZrDlCmdDrawImage.Format)}"] =
                GetOffset<ZrDlCmdDrawImage>(nameof(ZrDlCmdDrawImage.Format)),
            [$"{nameof(ZrDlCmdDrawImage)}.{nameof(ZrDlCmdDrawImage.ZLayer)}"] =
                GetOffset<ZrDlCmdDrawImage>(nameof(ZrDlCmdDrawImage.ZLayer)),
            [$"{nameof(ZrDlCmdDrawImage)}.{nameof(ZrDlCmdDrawImage.Reserved1)}"] =
                GetOffset<ZrDlCmdDrawImage>(nameof(ZrDlCmdDrawImage.Reserved1))
        };

        // Assert
        AssertIntMap(expected, actual);
    }

    [Fact]
    public void DebugStructs_ShouldMatchExpectedSizes()
    {
        // Arrange
        var expected = new Dictionary<string, int>
        {
            [nameof(ZrDebugRecordHeader)] = 40,
            [nameof(ZrDebugFrameRecord)] = 56,
            [nameof(ZrDebugEventRecord)] = 32,
            [nameof(ZrDebugErrorRecord)] = 120,
            [nameof(ZrDebugDrawlistRecord)] = 48,
            [nameof(ZrDebugPerfRecord)] = 24,
            [nameof(ZrDebugConfig)] = 32,
            [nameof(ZrDebugQuery)] = 48,
            [nameof(ZrDebugQueryResult)] = 32,
            [nameof(ZrDebugStats)] = 32
        };

        // Act
        var actual = new Dictionary<string, int>
        {
            [nameof(ZrDebugRecordHeader)] = GetSize<ZrDebugRecordHeader>(),
            [nameof(ZrDebugFrameRecord)] = GetSize<ZrDebugFrameRecord>(),
            [nameof(ZrDebugEventRecord)] = GetSize<ZrDebugEventRecord>(),
            [nameof(ZrDebugErrorRecord)] = GetSize<ZrDebugErrorRecord>(),
            [nameof(ZrDebugDrawlistRecord)] = GetSize<ZrDebugDrawlistRecord>(),
            [nameof(ZrDebugPerfRecord)] = GetSize<ZrDebugPerfRecord>(),
            [nameof(ZrDebugConfig)] = GetSize<ZrDebugConfig>(),
            [nameof(ZrDebugQuery)] = GetSize<ZrDebugQuery>(),
            [nameof(ZrDebugQueryResult)] = GetSize<ZrDebugQueryResult>(),
            [nameof(ZrDebugStats)] = GetSize<ZrDebugStats>()
        };

        // Assert
        AssertIntMap(expected, actual);
    }

    [Fact]
    public void DebugStructs_ShouldMatchExpectedOffsets()
    {
        // Arrange
        var expected = new Dictionary<string, int>
        {
            [$"{nameof(ZrDebugRecordHeader)}.{nameof(ZrDebugRecordHeader.Category)}"] = 24,
            [$"{nameof(ZrDebugRecordHeader)}.{nameof(ZrDebugRecordHeader.PayloadSize)}"] = 36,
            [$"{nameof(ZrDebugErrorRecord)}.{nameof(ZrDebugErrorRecord.ErrorCode)}"] = 8,
            [$"{nameof(ZrDebugErrorRecord)}.{nameof(ZrDebugErrorRecord.SourceLine)}"] = 12,
            [$"{nameof(ZrDebugErrorRecord)}.{nameof(ZrDebugErrorRecord.OccurrenceCount)}"] = 16,
            [$"{nameof(ZrDebugErrorRecord)}.{nameof(ZrDebugErrorRecord.Message)}"] = 56,
            [$"{nameof(ZrDebugQuery)}.{nameof(ZrDebugQuery.CategoryMask)}"] = 32,
            [$"{nameof(ZrDebugQueryResult)}.{nameof(ZrDebugQueryResult.RecordsDropped)}"] = 24
        };

        // Act
        var actual = new Dictionary<string, int>
        {
            [$"{nameof(ZrDebugRecordHeader)}.{nameof(ZrDebugRecordHeader.Category)}"] =
                GetOffset<ZrDebugRecordHeader>(nameof(ZrDebugRecordHeader.Category)),
            [$"{nameof(ZrDebugRecordHeader)}.{nameof(ZrDebugRecordHeader.PayloadSize)}"] =
                GetOffset<ZrDebugRecordHeader>(nameof(ZrDebugRecordHeader.PayloadSize)),
            [$"{nameof(ZrDebugErrorRecord)}.{nameof(ZrDebugErrorRecord.ErrorCode)}"] =
                GetOffset<ZrDebugErrorRecord>(nameof(ZrDebugErrorRecord.ErrorCode)),
            [$"{nameof(ZrDebugErrorRecord)}.{nameof(ZrDebugErrorRecord.SourceLine)}"] =
                GetOffset<ZrDebugErrorRecord>(nameof(ZrDebugErrorRecord.SourceLine)),
            [$"{nameof(ZrDebugErrorRecord)}.{nameof(ZrDebugErrorRecord.OccurrenceCount)}"] =
                GetOffset<ZrDebugErrorRecord>(nameof(ZrDebugErrorRecord.OccurrenceCount)),
            [$"{nameof(ZrDebugErrorRecord)}.{nameof(ZrDebugErrorRecord.Message)}"] =
                GetOffset<ZrDebugErrorRecord>(nameof(ZrDebugErrorRecord.Message)),
            [$"{nameof(ZrDebugQuery)}.{nameof(ZrDebugQuery.CategoryMask)}"] =
                GetOffset<ZrDebugQuery>(nameof(ZrDebugQuery.CategoryMask)),
            [$"{nameof(ZrDebugQueryResult)}.{nameof(ZrDebugQueryResult.RecordsDropped)}"] =
                GetOffset<ZrDebugQueryResult>(nameof(ZrDebugQueryResult.RecordsDropped))
        };

        // Assert
        AssertIntMap(expected, actual);
    }

    [Fact]
    public void TerminalStructs_ShouldMatchExpectedLayout()
    {
        // Arrange
        var expected = new Dictionary<string, int>
        {
            [nameof(ZrTerminalProfile)] = 100,
            [$"{nameof(ZrTerminalProfile)}.{nameof(ZrTerminalProfile.VersionString)}"] = 7,
            [$"{nameof(ZrTerminalProfile)}.{nameof(ZrTerminalProfile.CellWidthPx)}"] = 88,
            [$"{nameof(ZrTerminalProfile)}.{nameof(ZrTerminalProfile.XtVersionResponded)}"] = 96,
            [nameof(ZrTerminalCaps)] = 36,
            [$"{nameof(ZrTerminalCaps)}.{nameof(ZrTerminalCaps.SgrAttrsSupported)}"] = 12,
            [$"{nameof(ZrTerminalCaps)}.{nameof(ZrTerminalCaps.TerminalId)}"] = 16,
            [$"{nameof(ZrTerminalCaps)}.{nameof(ZrTerminalCaps.CapFlags)}"] = 24
        };

        // Act
        var actual = new Dictionary<string, int>
        {
            [nameof(ZrTerminalProfile)] = GetSize<ZrTerminalProfile>(),
            [$"{nameof(ZrTerminalProfile)}.{nameof(ZrTerminalProfile.VersionString)}"] =
                GetOffset<ZrTerminalProfile>(nameof(ZrTerminalProfile.VersionString)),
            [$"{nameof(ZrTerminalProfile)}.{nameof(ZrTerminalProfile.CellWidthPx)}"] =
                GetOffset<ZrTerminalProfile>(nameof(ZrTerminalProfile.CellWidthPx)),
            [$"{nameof(ZrTerminalProfile)}.{nameof(ZrTerminalProfile.XtVersionResponded)}"] =
                GetOffset<ZrTerminalProfile>(nameof(ZrTerminalProfile.XtVersionResponded)),
            [nameof(ZrTerminalCaps)] = GetSize<ZrTerminalCaps>(),
            [$"{nameof(ZrTerminalCaps)}.{nameof(ZrTerminalCaps.SgrAttrsSupported)}"] =
                GetOffset<ZrTerminalCaps>(nameof(ZrTerminalCaps.SgrAttrsSupported)),
            [$"{nameof(ZrTerminalCaps)}.{nameof(ZrTerminalCaps.TerminalId)}"] =
                GetOffset<ZrTerminalCaps>(nameof(ZrTerminalCaps.TerminalId)),
            [$"{nameof(ZrTerminalCaps)}.{nameof(ZrTerminalCaps.CapFlags)}"] =
                GetOffset<ZrTerminalCaps>(nameof(ZrTerminalCaps.CapFlags))
        };

        // Assert
        AssertIntMap(expected, actual);
    }

    [Fact]
    public void EnumUnderlyingTypes_ShouldMatchAbiWidths()
    {
        // Arrange
        var expected = new Dictionary<string, Type>
        {
            [nameof(ZrPlatformColorMode)] = typeof(byte),
            [nameof(ZrTerminalId)] = typeof(int),
            [nameof(ZrTerminalCapFlags)] = typeof(uint),
            [nameof(ZrDebugCategoryMask)] = typeof(uint),
            [nameof(ZrEventType)] = typeof(uint),
            [nameof(ZrKey)] = typeof(uint),
            [nameof(ZrDebugSeverity)] = typeof(uint),
            [nameof(ZrDlOpcode)] = typeof(ushort),
            [nameof(ZrDlDrawImageProtocol)] = typeof(byte),
            [nameof(ZrDlDrawImageZLayer)] = typeof(sbyte)
        };

        // Act
        var actual = new Dictionary<string, Type>
        {
            [nameof(ZrPlatformColorMode)] = Enum.GetUnderlyingType(typeof(ZrPlatformColorMode)),
            [nameof(ZrTerminalId)] = Enum.GetUnderlyingType(typeof(ZrTerminalId)),
            [nameof(ZrTerminalCapFlags)] = Enum.GetUnderlyingType(typeof(ZrTerminalCapFlags)),
            [nameof(ZrDebugCategoryMask)] = Enum.GetUnderlyingType(typeof(ZrDebugCategoryMask)),
            [nameof(ZrEventType)] = Enum.GetUnderlyingType(typeof(ZrEventType)),
            [nameof(ZrKey)] = Enum.GetUnderlyingType(typeof(ZrKey)),
            [nameof(ZrDebugSeverity)] = Enum.GetUnderlyingType(typeof(ZrDebugSeverity)),
            [nameof(ZrDlOpcode)] = Enum.GetUnderlyingType(typeof(ZrDlOpcode)),
            [nameof(ZrDlDrawImageProtocol)] = Enum.GetUnderlyingType(typeof(ZrDlDrawImageProtocol)),
            [nameof(ZrDlDrawImageZLayer)] = Enum.GetUnderlyingType(typeof(ZrDlDrawImageZLayer))
        };

        // Assert
        AssertTypeMap(expected, actual);
    }

    private static int GetSize<T>() where T : struct => Marshal.SizeOf(typeof(T));

    private static int GetOffset<T>(string fieldName) where T : struct =>
        checked((int)Marshal.OffsetOf(typeof(T), fieldName));

    private static void AssertIntMap(IReadOnlyDictionary<string, int> expected, IReadOnlyDictionary<string, int> actual)
    {
        Assert.Equal(expected.Count, actual.Count);
        foreach (var entry in expected)
        {
            Assert.True(actual.ContainsKey(entry.Key), $"Missing actual key: {entry.Key}");
            Assert.Equal(entry.Value, actual[entry.Key]);
        }
    }

    private static void AssertTypeMap(IReadOnlyDictionary<string, Type> expected,
        IReadOnlyDictionary<string, Type> actual)
    {
        Assert.Equal(expected.Count, actual.Count);
        foreach (var entry in expected)
        {
            Assert.True(actual.ContainsKey(entry.Key), $"Missing actual key: {entry.Key}");
            Assert.Equal(entry.Value, actual[entry.Key]);
        }
    }
}