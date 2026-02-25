using System;
using System.Runtime.InteropServices;
using Zireael.Net.Interop;

namespace Zireael.Net.Internal;

internal sealed class ZrEngineSafeHandle : SafeHandle
{
    private ZrEngineSafeHandle()
        : base(IntPtr.Zero, ownsHandle: true)
    {
    }

    internal ZrEngineSafeHandle(IntPtr handle)
        : base(IntPtr.Zero, ownsHandle: true)
    {
        SetHandle(handle);
    }

    public override bool IsInvalid => handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        ZrNative.engine_destroy(handle);
        return true;
    }
}