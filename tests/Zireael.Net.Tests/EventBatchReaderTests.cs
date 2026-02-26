using System.Text;

namespace Zireael.Net.Tests;

public class EventBatchReaderTests
{
    [Fact]
    public void TryCreate_WithValidBatch_ShouldParseHeaderAndRecords()
    {
        // Arrange
        var keyPayload = BuildKeyPayload(ZrKey.F1, ZrModifiers.Ctrl, ZrKeyAction.Down);
        var userBytes = Encoding.UTF8.GetBytes("abc");
        var userPayload = BuildUserPayload(42u, userBytes);
        var batch = BuildBatch(
            BuildRecord(ZrEventType.Key, 11u, 0u, keyPayload),
            BuildRecord(ZrEventType.User, 12u, 0u, userPayload));

        // Act
        var parsed = ZrEventBatchReader.TryCreate(batch, out var reader);

        // Assert
        Assert.True(parsed);
        Assert.NotNull(reader);
        Assert.Equal((uint)2, reader!.Header.EventCount);
        Assert.Equal(2, reader.RecordCount);

        var keyRecord = reader.GetRecord(0);
        Assert.True(keyRecord.TryGetKey(out var key));
        Assert.Equal(ZrKey.F1, key.Key);
        Assert.Equal(ZrModifiers.Ctrl, key.Mods);
        Assert.Equal(ZrKeyAction.Down, key.Action);

        var userRecord = reader.GetRecord(1);
        Assert.True(userRecord.TryGetUser(out var user, out var userPayloadBytes));
        Assert.Equal((uint)42, user.Tag);
        Assert.Equal((uint)3, user.ByteLen);
        Assert.Equal("abc", Encoding.UTF8.GetString(userPayloadBytes.Array!, userPayloadBytes.Offset, userPayloadBytes.Count));
    }

    [Fact]
    public void TryCreate_WithInvalidMagic_ShouldReturnFalse()
    {
        // Arrange
        var batch = BuildBatch(BuildRecord(ZrEventType.Tick, 1u, 0u, BuildTickPayload(16)));
        WriteU32(batch, 0, 0u);

        // Act
        var parsed = ZrEventBatchReader.TryCreate(batch, out var reader);

        // Assert
        Assert.False(parsed);
        Assert.Null(reader);
    }

    [Fact]
    public void TryGetKey_WhenRecordTypeDiffers_ShouldReturnFalse()
    {
        // Arrange
        var batch = BuildBatch(BuildRecord(ZrEventType.Resize, 1u, 0u, BuildResizePayload(120u, 40u)));
        var parsed = ZrEventBatchReader.TryCreate(batch, out var reader);
        Assert.True(parsed);

        // Act
        var canReadKey = reader!.GetRecord(0).TryGetKey(out _);

        // Assert
        Assert.False(canReadKey);
    }

    [Fact]
    public void TryReadHeader_WithUnsupportedVersion_ShouldReturnFalse()
    {
        // Arrange
        var batch = BuildBatch(BuildRecord(ZrEventType.Tick, 1u, 0u, BuildTickPayload(16)));
        WriteU32(batch, 4, 99u);

        // Act
        var parsed = ZrEventBatchReader.TryReadHeader(batch, out _);

        // Assert
        Assert.False(parsed);
    }

    private static byte[] BuildBatch(params byte[][] records)
    {
        var totalSize = 24;
        for (var i = 0; i < records.Length; i++)
        {
            totalSize += records[i].Length;
        }

        var batch = new byte[totalSize];
        WriteU32(batch, 0, ZrEventConstants.Magic);
        WriteU32(batch, 4, ZrVersion.EventBatchVersionV1);
        WriteU32(batch, 8, (uint)totalSize);
        WriteU32(batch, 12, (uint)records.Length);
        WriteU32(batch, 16, 0);
        WriteU32(batch, 20, 0);

        var offset = 24;
        for (var i = 0; i < records.Length; i++)
        {
            var record = records[i];
            Buffer.BlockCopy(record, 0, batch, offset, record.Length);
            offset += record.Length;
        }

        return batch;
    }

    private static byte[] BuildRecord(ZrEventType type, uint timeMs, uint flags, byte[] payload)
    {
        var rawSize = 16 + payload.Length;
        var size = Align4(rawSize);
        var record = new byte[size];

        WriteU32(record, 0, (uint)type);
        WriteU32(record, 4, (uint)size);
        WriteU32(record, 8, timeMs);
        WriteU32(record, 12, flags);
        Buffer.BlockCopy(payload, 0, record, 16, payload.Length);

        return record;
    }

    private static byte[] BuildKeyPayload(ZrKey key, ZrModifiers mods, ZrKeyAction action)
    {
        var bytes = new byte[16];
        WriteU32(bytes, 0, (uint)key);
        WriteU32(bytes, 4, (uint)mods);
        WriteU32(bytes, 8, (uint)action);
        WriteU32(bytes, 12, 0);
        return bytes;
    }

    private static byte[] BuildUserPayload(uint tag, byte[] userBytes)
    {
        var bytes = new byte[16 + userBytes.Length];
        WriteU32(bytes, 0, tag);
        WriteU32(bytes, 4, (uint)userBytes.Length);
        WriteU32(bytes, 8, 0);
        WriteU32(bytes, 12, 0);
        Buffer.BlockCopy(userBytes, 0, bytes, 16, userBytes.Length);
        return bytes;
    }

    private static byte[] BuildResizePayload(uint cols, uint rows)
    {
        var bytes = new byte[16];
        WriteU32(bytes, 0, cols);
        WriteU32(bytes, 4, rows);
        WriteU32(bytes, 8, 0);
        WriteU32(bytes, 12, 0);
        return bytes;
    }

    private static byte[] BuildTickPayload(uint dtMs)
    {
        var bytes = new byte[16];
        WriteU32(bytes, 0, dtMs);
        WriteU32(bytes, 4, 0);
        WriteU32(bytes, 8, 0);
        WriteU32(bytes, 12, 0);
        return bytes;
    }

    private static int Align4(int value) => (value + 3) & ~3;

    private static void WriteU32(byte[] destination, int offset, uint value)
    {
        destination[offset] = (byte)(value & 0xFF);
        destination[offset + 1] = (byte)((value >> 8) & 0xFF);
        destination[offset + 2] = (byte)((value >> 16) & 0xFF);
        destination[offset + 3] = (byte)((value >> 24) & 0xFF);
    }
}
