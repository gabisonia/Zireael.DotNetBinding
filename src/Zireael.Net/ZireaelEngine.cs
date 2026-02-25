using System;
using System.Runtime.InteropServices;
using Zireael.Net.Internal;
using Zireael.Net.Interop;

namespace Zireael.Net;

public sealed class ZireaelEngine : IDisposable
{
    private readonly ZrEngineSafeHandle _handle;
    private bool _disposed;

    private ZireaelEngine(ZrEngineSafeHandle handle)
    {
        _handle = handle;
    }

    private static ZrEngineConfig GetDefaultConfig() => ZrNative.zr_engine_config_default();

    public static ZrEngineConfig CreatePinnedDefaultConfig(uint drawlistVersion = ZrVersion.DrawlistVersionV5)
    {
        var cfg = GetDefaultConfig();
        cfg.RequestedEngineAbiMajor = ZrVersion.EngineAbiMajor;
        cfg.RequestedEngineAbiMinor = ZrVersion.EngineAbiMinor;
        cfg.RequestedEngineAbiPatch = ZrVersion.EngineAbiPatch;
        cfg.RequestedDrawlistVersion = drawlistVersion;
        cfg.RequestedEventBatchVersion = ZrVersion.EventBatchVersionV1;
        return cfg;
    }

    public static ZrEngineRuntimeConfig CreateRuntimeConfigFrom(in ZrEngineConfig cfg)
    {
        return new ZrEngineRuntimeConfig
        {
            Limits = cfg.Limits,
            Platform = cfg.Platform,
            TabWidth = cfg.TabWidth,
            WidthPolicy = cfg.WidthPolicy,
            TargetFps = cfg.TargetFps,
            EnableScrollOptimizations = cfg.EnableScrollOptimizations,
            EnableDebugOverlay = cfg.EnableDebugOverlay,
            EnableReplayRecording = cfg.EnableReplayRecording,
            WaitForOutputDrain = cfg.WaitForOutputDrain,
            CapForceFlags = cfg.CapForceFlags,
            CapSuppressFlags = cfg.CapSuppressFlags
        };
    }

    public static ZrLimits GetDefaultLimits() => ZrNative.zr_limits_default();

    private static ZrDebugConfig GetDefaultDebugConfig() => ZrNative.zr_debug_config_default();

    public static ZrResult ValidateConfig(in ZrEngineConfig cfg) => ZrNative.zr_engine_config_validate(in cfg);

    public static ZrResult ValidateRuntimeConfig(in ZrEngineRuntimeConfig cfg) =>
        ZrNative.zr_engine_runtime_config_validate(in cfg);

    public static ZrResult ValidateLimits(in ZrLimits limits) => ZrNative.zr_limits_validate(in limits);

    public static ZireaelEngine Create(in ZrEngineConfig cfg)
    {
        var rc = ZrNative.engine_create(out var enginePtr, in cfg);
        if (rc != ZrResult.Ok || enginePtr == IntPtr.Zero)
        {
            throw new ZireaelException(rc);
        }

        return new ZireaelEngine(new ZrEngineSafeHandle(enginePtr));
    }

    public static ZireaelEngine CreateDefault(uint drawlistVersion = ZrVersion.DrawlistVersionV5)
    {
        var cfg = CreatePinnedDefaultConfig(drawlistVersion);
        return Create(in cfg);
    }

    public int PollEvents(int timeoutMs, byte[] destination)
    {
        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        EnsureNotDisposed();

        unsafe
        {
            fixed (byte* dst = destination)
            {
                var ptr = destination.Length == 0 ? null : dst;
                var written = ZrNative.engine_poll_events(_handle, timeoutMs, ptr, destination.Length);
                if (written < 0)
                {
                    throw new ZireaelException((ZrResult)written);
                }

                return written;
            }
        }
    }

    public int PollEvents(int timeoutMs)
    {
        EnsureNotDisposed();

        unsafe
        {
            var written = ZrNative.engine_poll_events(_handle, timeoutMs, null, 0);
            if (written < 0)
            {
                throw new ZireaelException((ZrResult)written);
            }

            return written;
        }
    }

    public void PostUserEvent(uint tag, byte[]? payload)
    {
        EnsureNotDisposed();

        unsafe
        {
            if (payload == null || payload.Length == 0)
            {
                ThrowIfFailed(ZrNative.engine_post_user_event(_handle, tag, null, 0));
                return;
            }

            fixed (byte* payloadPtr = payload)
            {
                ThrowIfFailed(ZrNative.engine_post_user_event(_handle, tag, payloadPtr, payload.Length));
            }
        }
    }

    public void SubmitDrawlist(byte[] drawlistBytes)
    {
        if (drawlistBytes == null)
        {
            throw new ArgumentNullException(nameof(drawlistBytes));
        }

        EnsureNotDisposed();

        unsafe
        {
            fixed (byte* bytes = drawlistBytes)
            {
                var ptr = drawlistBytes.Length == 0 ? null : bytes;
                ThrowIfFailed(ZrNative.engine_submit_drawlist(_handle, ptr, drawlistBytes.Length));
            }
        }
    }

    public void Present()
    {
        EnsureNotDisposed();
        ThrowIfFailed(ZrNative.engine_present(_handle));
    }

    public ZrMetrics GetMetrics()
    {
        EnsureNotDisposed();

        var metrics = new ZrMetrics
        {
            StructSize = (uint)Marshal.SizeOf(typeof(ZrMetrics))
        };

        ThrowIfFailed(ZrNative.engine_get_metrics(_handle, ref metrics));
        return metrics;
    }

    public ZrTerminalCaps GetCapabilities()
    {
        EnsureNotDisposed();
        ThrowIfFailed(ZrNative.engine_get_caps(_handle, out var caps));
        return caps;
    }

    public ZrTerminalProfile GetTerminalProfile()
    {
        EnsureNotDisposed();

        var ptr = ZrNative.engine_get_terminal_profile(_handle);
        if (ptr == IntPtr.Zero)
        {
            throw new ZireaelException("engine_get_terminal_profile returned a null pointer.");
        }

        return (ZrTerminalProfile)Marshal.PtrToStructure(ptr, typeof(ZrTerminalProfile));
    }

    public void SetConfig(in ZrEngineRuntimeConfig cfg)
    {
        EnsureNotDisposed();
        ThrowIfFailed(ZrNative.engine_set_config(_handle, in cfg));
    }

    public void EnableDebug()
    {
        var cfg = GetDefaultDebugConfig();
        EnableDebug(in cfg);
    }

    public void EnableDebug(in ZrDebugConfig cfg)
    {
        EnsureNotDisposed();
        ThrowIfFailed(ZrNative.engine_debug_enable(_handle, in cfg));
    }

    public void DisableDebug()
    {
        EnsureNotDisposed();
        ZrNative.engine_debug_disable(_handle);
    }

    public ZrDebugQueryResult DebugQuery(in ZrDebugQuery query, ZrDebugRecordHeader[]? destination)
    {
        EnsureNotDisposed();

        unsafe
        {
            var cap = destination == null ? 0u : (uint)destination.Length;

            fixed (ZrDebugRecordHeader* ptr = destination)
            {
                ThrowIfFailed(ZrNative.engine_debug_query(_handle, in query, ptr, cap, out var result));
                return result;
            }
        }
    }

    public ZrDebugQueryResult DebugQuery(in ZrDebugQuery query)
    {
        EnsureNotDisposed();

        unsafe
        {
            ThrowIfFailed(ZrNative.engine_debug_query(_handle, in query, null, 0, out var result));
            return result;
        }
    }

    public uint DebugGetPayload(ulong recordId, byte[]? destination)
    {
        EnsureNotDisposed();

        unsafe
        {
            if (destination == null || destination.Length == 0)
            {
                ThrowIfFailed(ZrNative.engine_debug_get_payload(_handle, recordId, null, 0, out var required));
                return required;
            }

            fixed (byte* ptr = destination)
            {
                ThrowIfFailed(ZrNative.engine_debug_get_payload(_handle, recordId, ptr, (uint)destination.Length,
                    out var written));
                return written;
            }
        }
    }

    public ZrDebugStats GetDebugStats()
    {
        EnsureNotDisposed();
        ThrowIfFailed(ZrNative.engine_debug_get_stats(_handle, out var stats));
        return stats;
    }

    public int DebugExport(byte[]? destination)
    {
        EnsureNotDisposed();

        unsafe
        {
            if (destination == null || destination.Length == 0)
            {
                var bytesNoBuffer = ZrNative.engine_debug_export(_handle, null, 0);
                if (bytesNoBuffer < 0)
                {
                    throw new ZireaelException((ZrResult)bytesNoBuffer);
                }

                return bytesNoBuffer;
            }

            fixed (byte* ptr = destination)
            {
                var bytes = ZrNative.engine_debug_export(_handle, ptr, (nuint)destination.Length);
                if (bytes < 0)
                {
                    throw new ZireaelException((ZrResult)bytes);
                }

                return bytes;
            }
        }
    }

    public void DebugReset()
    {
        EnsureNotDisposed();
        ZrNative.engine_debug_reset(_handle);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _handle.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    private void EnsureNotDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(ZireaelEngine));
        }
    }

    private static void ThrowIfFailed(ZrResult rc)
    {
        if (rc != ZrResult.Ok)
        {
            throw new ZireaelException(rc);
        }
    }
}