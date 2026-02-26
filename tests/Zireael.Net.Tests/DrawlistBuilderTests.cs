using System.Text;

namespace Zireael.Net.Tests;

public class DrawlistBuilderTests
{
    [Fact]
    public void Build_WithClearAndDrawText_ShouldProduceValidV1Payload()
    {
        // Arrange
        var builder = new ZrDrawlistBuilder();
        var style = new ZrDlStyle
        {
            Fg = 0x00112233,
            Bg = 0x00445566,
            Attrs = 0x00000007,
            Reserved0 = 0
        };
        const string text = "Hi";
        var textBytes = Encoding.UTF8.GetBytes(text);

        builder.Clear();
        builder.DrawText(5, 7, text, in style);

        // Act
        var bytes = builder.Build();

        // Assert
        Assert.Equal((uint)132, (uint)bytes.Length);
        Assert.Equal(ZrDrawlistConstants.Magic, ReadU32(bytes, 0));
        Assert.Equal(ZrVersion.DrawlistVersionV1, ReadU32(bytes, 4));
        Assert.Equal((uint)64, ReadU32(bytes, 8));
        Assert.Equal((uint)132, ReadU32(bytes, 12));
        Assert.Equal((uint)64, ReadU32(bytes, 16));
        Assert.Equal((uint)56, ReadU32(bytes, 20));
        Assert.Equal((uint)2, ReadU32(bytes, 24));
        Assert.Equal((uint)120, ReadU32(bytes, 28));
        Assert.Equal((uint)1, ReadU32(bytes, 32));
        Assert.Equal((uint)128, ReadU32(bytes, 36));
        Assert.Equal((uint)4, ReadU32(bytes, 40));

        Assert.Equal((ushort)ZrDlOpcode.Clear, ReadU16(bytes, 64));
        Assert.Equal((uint)8, ReadU32(bytes, 68));

        Assert.Equal((ushort)ZrDlOpcode.DrawText, ReadU16(bytes, 72));
        Assert.Equal((uint)48, ReadU32(bytes, 76));
        Assert.Equal(5, ReadI32(bytes, 80));
        Assert.Equal(7, ReadI32(bytes, 84));
        Assert.Equal((uint)0, ReadU32(bytes, 88)); // string index
        Assert.Equal((uint)0, ReadU32(bytes, 92)); // byte off
        Assert.Equal((uint)textBytes.Length, ReadU32(bytes, 96));
        Assert.Equal(style.Fg, ReadU32(bytes, 100));
        Assert.Equal(style.Bg, ReadU32(bytes, 104));
        Assert.Equal(style.Attrs, ReadU32(bytes, 108));
        Assert.Equal(style.Reserved0, ReadU32(bytes, 112));
        Assert.Equal((uint)0, ReadU32(bytes, 116)); // cmd reserved0

        Assert.Equal((uint)0, ReadU32(bytes, 120)); // span off
        Assert.Equal((uint)textBytes.Length, ReadU32(bytes, 124)); // span len
        Assert.Equal(textBytes[0], bytes[128]);
        Assert.Equal(textBytes[1], bytes[129]);
        Assert.Equal((byte)0, bytes[130]); // alignment padding
        Assert.Equal((byte)0, bytes[131]); // alignment padding
    }

    [Fact]
    public void Build_WithUnsupportedVersion_ShouldThrow()
    {
        // Arrange
        var builder = new ZrDrawlistBuilder();
        builder.Clear();
        const uint unsupportedVersion = ZrVersion.DrawlistVersionV3;

        // Act
        var act = () => builder.Build(unsupportedVersion);

        // Assert
        Assert.Throws<NotSupportedException>(act);
    }

    [Fact]
    public void Reset_ShouldClearPreviouslyQueuedData()
    {
        // Arrange
        var builder = new ZrDrawlistBuilder();
        builder.DrawText(1, 1, "stale");

        // Act
        builder.Reset();
        builder.Clear();
        var bytes = builder.Build();

        // Assert
        Assert.Equal((uint)72, (uint)bytes.Length); // 64 header + 8 clear command
        Assert.Equal((uint)1, ReadU32(bytes, 24)); // cmd_count
        Assert.Equal((uint)0, ReadU32(bytes, 32)); // strings_count
        Assert.Equal((uint)0, ReadU32(bytes, 40)); // strings_bytes_len
    }

    private static ushort ReadU16(byte[] source, int offset)
    {
        return (ushort)(source[offset] | (source[offset + 1] << 8));
    }

    private static uint ReadU32(byte[] source, int offset)
    {
        return (uint)(
            source[offset] |
            (source[offset + 1] << 8) |
            (source[offset + 2] << 16) |
            (source[offset + 3] << 24));
    }

    private static int ReadI32(byte[] source, int offset) => unchecked((int)ReadU32(source, offset));
}
