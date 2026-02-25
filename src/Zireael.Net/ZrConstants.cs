namespace Zireael.Net;

/// <summary>
/// Constants for event-batch wire format.
/// </summary>
public static class ZrEventConstants
{
    /// <summary>
    /// Magic value at the start of an event batch header.
    /// </summary>
    public const uint Magic = 0x5645525A;

    /// <summary>
    /// Event-batch flag indicating truncation.
    /// </summary>
    public const uint BatchTruncated = 1u << 0;
}

/// <summary>
/// Constants for debug records and buffers.
/// </summary>
public static class ZrDebugConstants
{
    /// <summary>
    /// Fixed byte length of the source-file field in debug error records.
    /// </summary>
    public const int SourceFileLength = 32;

    /// <summary>
    /// Fixed byte length of the message field in debug error records.
    /// </summary>
    public const int MessageLength = 64;
}

/// <summary>
/// Constants related to terminal profile buffers.
/// </summary>
public static class ZrTerminalConstants
{
    /// <summary>
    /// Fixed byte length of the terminal version string field.
    /// </summary>
    public const int VersionLength = 64;
}

/// <summary>
/// Constants for drawlist wire format.
/// </summary>
public static class ZrDrawlistConstants
{
    // Defined in docs/abi/drawlist-format.md as little-endian bytes for "ZRDL".
    /// <summary>
    /// Magic value at the start of a drawlist header.
    /// </summary>
    public const uint Magic = 0x4C44525A;
}