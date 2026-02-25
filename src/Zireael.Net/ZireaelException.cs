using System;

namespace Zireael.Net;

/// <summary>
/// Exception raised when a Zireael API call fails.
/// </summary>
public sealed class ZireaelException : Exception
{
    /// <summary>
    /// Initializes an exception from a native result code.
    /// </summary>
    /// <param name="result">Native result code.</param>
    public ZireaelException(ZrResult result)
        : base($"Zireael call failed with {result} ({(int)result}).")
    {
        Result = result;
    }

    /// <summary>
    /// Initializes an exception with a custom message.
    /// </summary>
    /// <param name="message">Error message.</param>
    public ZireaelException(string message)
        : base(message)
    {
        Result = ZrResult.ErrPlatform;
    }

    /// <summary>
    /// Gets the result code associated with the failure.
    /// </summary>
    public ZrResult Result { get; }
}