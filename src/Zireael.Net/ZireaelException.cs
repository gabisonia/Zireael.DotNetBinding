using System;

namespace Zireael.Net;

public sealed class ZireaelException : Exception
{
    public ZireaelException(ZrResult result)
        : base($"Zireael call failed with {result} ({(int)result}).")
    {
        Result = result;
    }

    public ZireaelException(string message)
        : base(message)
    {
        Result = ZrResult.ErrPlatform;
    }

    public ZrResult Result { get; }
}