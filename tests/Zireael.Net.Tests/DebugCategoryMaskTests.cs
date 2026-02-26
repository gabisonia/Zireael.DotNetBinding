namespace Zireael.Net.Tests;

public class DebugCategoryMaskTests
{
    [Fact]
    public void ForCategory_ShouldMapOrdinalsToShiftedBits()
    {
        // Arrange
        const ZrDebugCategoryMask expectedFrame = (ZrDebugCategoryMask)(1u << 1);
        const ZrDebugCategoryMask expectedEvent = (ZrDebugCategoryMask)(1u << 2);
        const ZrDebugCategoryMask expectedPerf = (ZrDebugCategoryMask)(1u << 6);

        // Act
        var actualFrame = ZrDebugCategoryMaskHelpers.ForCategory(ZrDebugCategory.Frame);
        var actualEvent = ZrDebugCategoryMaskHelpers.ForCategory(ZrDebugCategory.Event);
        var actualPerf = ZrDebugCategoryMaskHelpers.ForCategory(ZrDebugCategory.Perf);
        var actualNone = ZrDebugCategoryMaskHelpers.ForCategory(ZrDebugCategory.None);

        // Assert
        Assert.Equal(expectedFrame, actualFrame);
        Assert.Equal(expectedEvent, actualEvent);
        Assert.Equal(expectedPerf, actualPerf);
        Assert.Equal(ZrDebugCategoryMask.None, actualNone);
    }

    [Fact]
    public void Includes_ShouldCheckCategoryBitUsingOrdinal()
    {
        // Arrange
        var mask = ZrDebugCategoryMask.Frame | ZrDebugCategoryMask.Error;

        // Act
        var includesFrame = mask.Includes(ZrDebugCategory.Frame);
        var includesError = mask.Includes(ZrDebugCategory.Error);
        var includesEvent = mask.Includes(ZrDebugCategory.Event);
        var includesNone = mask.Includes(ZrDebugCategory.None);

        // Assert
        Assert.True(includesFrame);
        Assert.True(includesError);
        Assert.False(includesEvent);
        Assert.False(includesNone);
    }

    [Fact]
    public void RawConverters_ShouldRoundTripMaskWithoutLoss()
    {
        // Arrange
        const uint raw = 0xFFFFFFFFu;
        var expectedMask = (ZrDebugCategoryMask)raw;

        // Act
        var typedMask = ZrDebugCategoryMaskHelpers.FromRaw(raw);
        var roundTripRaw = typedMask.ToRaw();

        // Assert
        Assert.Equal(expectedMask, typedMask);
        Assert.Equal(raw, roundTripRaw);
    }
}