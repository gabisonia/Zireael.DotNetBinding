namespace Zireael.Net.Tests;

public class RuntimeConfigAndExceptionTests
{
    [Fact]
    public void CreateRuntimeConfigFrom_ShouldCopyRuntimeRelevantFields()
    {
        // Arrange
        var config = new ZrEngineConfig
        {
            Limits = new ZrLimits
            {
                ArenaMaxTotalBytes = 100,
                ArenaInitialBytes = 101,
                OutMaxBytesPerFrame = 102,
                DlMaxTotalBytes = 103,
                DlMaxCmds = 104,
                DlMaxStrings = 105,
                DlMaxBlobs = 106,
                DlMaxClipDepth = 107,
                DlMaxTextRunSegments = 108,
                DiffMaxDamageRects = 109
            },
            Platform = new ZrPlatformConfig
            {
                RequestedColorMode = ZrPlatformColorMode.Color256,
                EnableMouse = 1,
                EnableBracketedPaste = 1,
                EnableFocusEvents = 0,
                EnableOsc52 = 1
            },
            TabWidth = 4,
            WidthPolicy = ZrWidthPolicy.EmojiWide,
            TargetFps = 144,
            EnableScrollOptimizations = 1,
            EnableDebugOverlay = 0,
            EnableReplayRecording = 1,
            WaitForOutputDrain = 1,
            CapForceFlags = ZrTerminalCapFlags.Sixel | ZrTerminalCapFlags.Hyperlinks,
            CapSuppressFlags = ZrTerminalCapFlags.Mouse
        };

        // Act
        var runtime = ZireaelEngine.CreateRuntimeConfigFrom(in config);

        // Assert
        Assert.Equal(config.Limits.ArenaMaxTotalBytes, runtime.Limits.ArenaMaxTotalBytes);
        Assert.Equal(config.Limits.ArenaInitialBytes, runtime.Limits.ArenaInitialBytes);
        Assert.Equal(config.Limits.OutMaxBytesPerFrame, runtime.Limits.OutMaxBytesPerFrame);
        Assert.Equal(config.Limits.DlMaxTotalBytes, runtime.Limits.DlMaxTotalBytes);
        Assert.Equal(config.Limits.DlMaxCmds, runtime.Limits.DlMaxCmds);
        Assert.Equal(config.Limits.DlMaxStrings, runtime.Limits.DlMaxStrings);
        Assert.Equal(config.Limits.DlMaxBlobs, runtime.Limits.DlMaxBlobs);
        Assert.Equal(config.Limits.DlMaxClipDepth, runtime.Limits.DlMaxClipDepth);
        Assert.Equal(config.Limits.DlMaxTextRunSegments, runtime.Limits.DlMaxTextRunSegments);
        Assert.Equal(config.Limits.DiffMaxDamageRects, runtime.Limits.DiffMaxDamageRects);

        Assert.Equal(config.Platform.RequestedColorMode, runtime.Platform.RequestedColorMode);
        Assert.Equal(config.Platform.EnableMouse, runtime.Platform.EnableMouse);
        Assert.Equal(config.Platform.EnableBracketedPaste, runtime.Platform.EnableBracketedPaste);
        Assert.Equal(config.Platform.EnableFocusEvents, runtime.Platform.EnableFocusEvents);
        Assert.Equal(config.Platform.EnableOsc52, runtime.Platform.EnableOsc52);

        Assert.Equal(config.TabWidth, runtime.TabWidth);
        Assert.Equal(config.WidthPolicy, runtime.WidthPolicy);
        Assert.Equal(config.TargetFps, runtime.TargetFps);
        Assert.Equal(config.EnableScrollOptimizations, runtime.EnableScrollOptimizations);
        Assert.Equal(config.EnableDebugOverlay, runtime.EnableDebugOverlay);
        Assert.Equal(config.EnableReplayRecording, runtime.EnableReplayRecording);
        Assert.Equal(config.WaitForOutputDrain, runtime.WaitForOutputDrain);
        Assert.Equal(config.CapForceFlags, runtime.CapForceFlags);
        Assert.Equal(config.CapSuppressFlags, runtime.CapSuppressFlags);
    }

    [Fact]
    public void ZireaelException_ResultConstructor_ShouldPreserveResultAndMessage()
    {
        // Arrange
        const ZrResult expectedResult = ZrResult.ErrUnsupported;

        // Act
        var ex = new ZireaelException(ZrResult.ErrUnsupported);

        // Assert
        Assert.Equal(expectedResult, ex.Result);
        Assert.Contains("ErrUnsupported", ex.Message);
        Assert.Contains("-4", ex.Message);
    }

    [Fact]
    public void ZireaelException_MessageConstructor_ShouldDefaultToPlatformResult()
    {
        // Arrange
        const string expectedMessage = "custom failure";
        const ZrResult expectedResult = ZrResult.ErrPlatform;

        // Act
        var ex = new ZireaelException(expectedMessage);

        // Assert
        Assert.Equal(expectedResult, ex.Result);
        Assert.Equal(expectedMessage, ex.Message);
    }
}