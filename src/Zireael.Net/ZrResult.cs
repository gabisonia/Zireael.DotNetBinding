namespace Zireael.Net;

/// <summary>
/// Result codes returned by native Zireael APIs.
/// </summary>
public enum ZrResult
{
    /// <summary>
    /// Operation completed successfully.
    /// </summary>
    Ok = 0,

    /// <summary>
    /// One or more arguments were invalid.
    /// </summary>
    ErrInvalidArgument = -1,

    /// <summary>
    /// Operation failed because memory allocation failed.
    /// </summary>
    ErrOutOfMemory = -2,

    /// <summary>
    /// Operation exceeded a configured or hard limit.
    /// </summary>
    ErrLimit = -3,

    /// <summary>
    /// Operation is not supported in the current environment.
    /// </summary>
    ErrUnsupported = -4,

    /// <summary>
    /// Data format or payload validation failed.
    /// </summary>
    ErrFormat = -5,

    /// <summary>
    /// Platform-specific failure occurred.
    /// </summary>
    ErrPlatform = -6
}