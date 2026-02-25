namespace Zireael.Net.Tests;

public class VersionAndConstantsTests
{
    [Fact]
    public void ZrVersion_ShouldMatchPinnedHeaderValues()
    {
        // Arrange
        const uint expectedLibraryMajor = 1u;
        const uint expectedLibraryMinor = 3u;
        const uint expectedLibraryPatch = 8u;
        const uint expectedEngineAbiMajor = 1u;
        const uint expectedEngineAbiMinor = 2u;
        const uint expectedEngineAbiPatch = 0u;
        const uint expectedEventBatchVersion = 1u;

        // Act
        var actualLibraryMajor = ZrVersion.LibraryMajor;
        var actualLibraryMinor = ZrVersion.LibraryMinor;
        var actualLibraryPatch = ZrVersion.LibraryPatch;
        var actualEngineAbiMajor = ZrVersion.EngineAbiMajor;
        var actualEngineAbiMinor = ZrVersion.EngineAbiMinor;
        var actualEngineAbiPatch = ZrVersion.EngineAbiPatch;
        var actualEventBatchVersion = ZrVersion.EventBatchVersionV1;

        // Assert
        Assert.Equal(expectedLibraryMajor, actualLibraryMajor);
        Assert.Equal(expectedLibraryMinor, actualLibraryMinor);
        Assert.Equal(expectedLibraryPatch, actualLibraryPatch);
        Assert.Equal(expectedEngineAbiMajor, actualEngineAbiMajor);
        Assert.Equal(expectedEngineAbiMinor, actualEngineAbiMinor);
        Assert.Equal(expectedEngineAbiPatch, actualEngineAbiPatch);
        Assert.Equal(expectedEventBatchVersion, actualEventBatchVersion);
    }

    [Fact]
    public void DrawlistVersions_ShouldBeSequential()
    {
        // Arrange
        var expected = new uint[] { 1u, 2u, 3u, 4u, 5u };

        // Act
        var actual = new uint[]
        {
            ZrVersion.DrawlistVersionV1,
            ZrVersion.DrawlistVersionV2,
            ZrVersion.DrawlistVersionV3,
            ZrVersion.DrawlistVersionV4,
            ZrVersion.DrawlistVersionV5
        };

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AllMask_ShouldEqualAllIndividualCapabilityBits()
    {
        // Arrange
        const ZrTerminalCapFlags expected =
            ZrTerminalCapFlags.Sixel |
            ZrTerminalCapFlags.KittyGraphics |
            ZrTerminalCapFlags.ITerm2Images |
            ZrTerminalCapFlags.UnderlineStyles |
            ZrTerminalCapFlags.ColoredUnderlines |
            ZrTerminalCapFlags.Hyperlinks |
            ZrTerminalCapFlags.GraphemeClusters |
            ZrTerminalCapFlags.Overline |
            ZrTerminalCapFlags.PixelMouse |
            ZrTerminalCapFlags.KittyKeyboard |
            ZrTerminalCapFlags.Mouse |
            ZrTerminalCapFlags.BracketedPaste |
            ZrTerminalCapFlags.FocusEvents |
            ZrTerminalCapFlags.Osc52 |
            ZrTerminalCapFlags.SyncUpdate |
            ZrTerminalCapFlags.ScrollRegion |
            ZrTerminalCapFlags.CursorShape |
            ZrTerminalCapFlags.OutputWaitWritable;

        // Act
        var actual = ZrTerminalCapFlags.AllMask;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MagicConstants_ShouldMatchWireFormatMarkers()
    {
        // Arrange
        const uint expectedEventMagic = 0x5645525Au;
        const uint expectedDrawlistMagic = 0x4C44525Au;

        // Act
        var actualEventMagic = ZrEventConstants.Magic;
        var actualDrawlistMagic = ZrDrawlistConstants.Magic;

        // Assert
        Assert.Equal(expectedEventMagic, actualEventMagic);
        Assert.Equal(expectedDrawlistMagic, actualDrawlistMagic);
    }
}
