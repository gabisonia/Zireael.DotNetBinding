namespace Zireael.Net;

/// <summary>
/// Version constants for the managed wrapper and supported native ABI revisions.
/// </summary>
public static class ZrVersion
{
    /// <summary>
    /// Managed wrapper major version.
    /// </summary>
    public const uint LibraryMajor = 1;

    /// <summary>
    /// Managed wrapper minor version.
    /// </summary>
    public const uint LibraryMinor = 3;

    /// <summary>
    /// Managed wrapper patch version.
    /// </summary>
    public const uint LibraryPatch = 8;

    /// <summary>
    /// Supported engine ABI major version.
    /// </summary>
    public const uint EngineAbiMajor = 1;

    /// <summary>
    /// Supported engine ABI minor version.
    /// </summary>
    public const uint EngineAbiMinor = 2;

    /// <summary>
    /// Supported engine ABI patch version.
    /// </summary>
    public const uint EngineAbiPatch = 0;

    /// <summary>
    /// Drawlist ABI version 1.
    /// </summary>
    public const uint DrawlistVersionV1 = 1;

    /// <summary>
    /// Drawlist ABI version 2.
    /// </summary>
    public const uint DrawlistVersionV2 = 2;

    /// <summary>
    /// Drawlist ABI version 3.
    /// </summary>
    public const uint DrawlistVersionV3 = 3;

    /// <summary>
    /// Drawlist ABI version 4.
    /// </summary>
    public const uint DrawlistVersionV4 = 4;

    /// <summary>
    /// Drawlist ABI version 5.
    /// </summary>
    public const uint DrawlistVersionV5 = 5;

    /// <summary>
    /// Event-batch ABI version 1.
    /// </summary>
    public const uint EventBatchVersionV1 = 1;
}