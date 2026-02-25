using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Zireael.Net.Internal;

internal static class Utf8Interop
{
    public static unsafe string ReadNullTerminated(byte* ptr, int maxLength)
    {
        var length = 0;
        while (length < maxLength && ptr[length] != 0)
        {
            length++;
        }

        if (length == 0)
        {
            return string.Empty;
        }

        var bytes = new byte[length];
        Marshal.Copy((IntPtr)ptr, bytes, 0, length);
        return Encoding.UTF8.GetString(bytes, 0, length);
    }
}
