using System;
using System.Runtime.InteropServices;
using Zireael.Net.Internal;
using Zireael.Net.Interop;

namespace Zireael.Net;

/// <summary>
/// Managed wrapper around a native Zireael engine instance.
/// </summary>
public sealed class ZireaelEngine : IDisposable
{
    private readonly ZrEngineSafeHandle _handle;
    private bool _disposed;

    private ZireaelEngine(ZrEngineSafeHandle handle)
    {
        _handle = handle;
    }

    private static ZrEngineConfig GetDefaultConfig() => ZrNative.zr_engine_config_default();

    /// <summary>
    /// Creates a default engine configuration pinned to this managed wrapper's ABI expectations.
    /// </summary>
    /// <param name="drawlistVersion">Requested drawlist ABI version.</param>
    /// <returns>A populated engine configuration.</returns>
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

    /// <summary>
    /// Creates a runtime configuration by copying runtime-tunable fields from an engine configuration.
    /// </summary>
    /// <param name="cfg">Source engine configuration.</param>
    /// <returns>A runtime configuration derived from <paramref name="cfg" />.</returns>
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

    /// <summary>
    /// Returns the native defaults for engine limits.
    /// </summary>
    /// <returns>A default limits value.</returns>
    public static ZrLimits GetDefaultLimits() => ZrNative.zr_limits_default();

    private static ZrDebugConfig GetDefaultDebugConfig() => ZrNative.zr_debug_config_default();

    /// <summary>
    /// Validates a full engine configuration.
    /// </summary>
    /// <param name="cfg">Configuration to validate.</param>
    /// <returns>The validation result code.</returns>
    public static ZrResult ValidateConfig(in ZrEngineConfig cfg) => ZrNative.zr_engine_config_validate(in cfg);

    /// <summary>
    /// Validates a runtime configuration.
    /// </summary>
    /// <param name="cfg">Runtime configuration to validate.</param>
    /// <returns>The validation result code.</returns>
    public static ZrResult ValidateRuntimeConfig(in ZrEngineRuntimeConfig cfg) =>
        ZrNative.zr_engine_runtime_config_validate(in cfg);

    /// <summary>
    /// Validates an engine limits structure.
    /// </summary>
    /// <param name="limits">Limits to validate.</param>
    /// <returns>The validation result code.</returns>
    public static ZrResult ValidateLimits(in ZrLimits limits) => ZrNative.zr_limits_validate(in limits);

    /// <summary>
    /// Creates an engine instance from the provided configuration.
    /// </summary>
    /// <param name="cfg">Configuration used to create the native engine.</param>
    /// <returns>An initialized engine instance.</returns>
    /// <exception cref="ZireaelException">Thrown when native engine creation fails.</exception>
    public static ZireaelEngine Create(in ZrEngineConfig cfg)
    {
        var rc = ZrNative.engine_create(out var enginePtr, in cfg);
        if (rc != ZrResult.Ok || enginePtr == IntPtr.Zero)
        {
            throw new ZireaelException(rc);
        }

        return new ZireaelEngine(new ZrEngineSafeHandle(enginePtr));
    }

    /// <summary>
    /// Creates an engine instance using pinned defaults.
    /// </summary>
    /// <param name="drawlistVersion">Requested drawlist ABI version.</param>
    /// <returns>An initialized engine instance.</returns>
    public static ZireaelEngine CreateDefault(uint drawlistVersion = ZrVersion.DrawlistVersionV5)
    {
        var cfg = CreatePinnedDefaultConfig(drawlistVersion);
        return Create(in cfg);
    }

    /// <summary>
    /// Polls events into a caller-provided buffer.
    /// </summary>
    /// <param name="timeoutMs">Maximum wait time in milliseconds.</param>
    /// <param name="destination">Buffer that receives encoded events.</param>
    /// <returns>Number of bytes written.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="destination" /> is <see langword="null" />.</exception>
    /// <exception cref="ZireaelException">Thrown when the native call fails.</exception>
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

    /// <summary>
    /// Polls events without reading payload bytes, returning only the required size.
    /// </summary>
    /// <param name="timeoutMs">Maximum wait time in milliseconds.</param>
    /// <returns>Number of bytes required for the current event batch.</returns>
    /// <exception cref="ZireaelException">Thrown when the native call fails.</exception>
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

    /// <summary>
    /// Posts a user-defined event payload.
    /// </summary>
    /// <param name="tag">Application-defined event tag.</param>
    /// <param name="payload">Optional event payload bytes.</param>
    /// <exception cref="ZireaelException">Thrown when the native call fails.</exception>
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

    /// <summary>
    /// Submits a drawlist payload for rendering.
    /// </summary>
    /// <param name="drawlistBytes">Serialized drawlist bytes.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="drawlistBytes" /> is <see langword="null" />.</exception>
    /// <exception cref="ZireaelException">Thrown when the native call fails.</exception>
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

    /// <summary>
    /// Presents the current frame.
    /// </summary>
    /// <exception cref="ZireaelException">Thrown when the native call fails.</exception>
    public void Present()
    {
        EnsureNotDisposed();
        ThrowIfFailed(ZrNative.engine_present(_handle));
    }

    /// <summary>
    /// Reads current engine metrics.
    /// </summary>
    /// <returns>Current metrics snapshot.</returns>
    /// <exception cref="ZireaelException">Thrown when the native call fails.</exception>
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

    /// <summary>
    /// Reads terminal capability flags detected by the engine.
    /// </summary>
    /// <returns>Current terminal capabilities.</returns>
    /// <exception cref="ZireaelException">Thrown when the native call fails.</exception>
    public ZrTerminalCaps GetCapabilities()
    {
        EnsureNotDisposed();
        ThrowIfFailed(ZrNative.engine_get_caps(_handle, out var caps));
        return caps;
    }

    /// <summary>
    /// Reads the detected terminal profile.
    /// </summary>
    /// <returns>Terminal profile information.</returns>
    /// <exception cref="ZireaelException">Thrown when the native call fails or returns a null profile pointer.</exception>
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

    /// <summary>
    /// Applies runtime configuration to the running engine.
    /// </summary>
    /// <param name="cfg">Runtime configuration to apply.</param>
    /// <exception cref="ZireaelException">Thrown when the native call fails.</exception>
    public void SetConfig(in ZrEngineRuntimeConfig cfg)
    {
        EnsureNotDisposed();
        ThrowIfFailed(ZrNative.engine_set_config(_handle, in cfg));
    }

    /// <summary>
    /// Enables debug capture using native defaults.
    /// </summary>
    public void EnableDebug()
    {
        var cfg = GetDefaultDebugConfig();
        EnableDebug(in cfg);
    }

    /// <summary>
    /// Enables debug capture with an explicit configuration.
    /// </summary>
    /// <param name="cfg">Debug configuration to apply.</param>
    /// <exception cref="ZireaelException">Thrown when the native call fails.</exception>
    public void EnableDebug(in ZrDebugConfig cfg)
    {
        EnsureNotDisposed();
        ThrowIfFailed(ZrNative.engine_debug_enable(_handle, in cfg));
    }

    /// <summary>
    /// Disables debug capture.
    /// </summary>
    public void DisableDebug()
    {
        EnsureNotDisposed();
        ZrNative.engine_debug_disable(_handle);
    }

    /// <summary>
    /// Queries debug records into a caller-provided header buffer.
    /// </summary>
    /// <param name="query">Query parameters.</param>
    /// <param name="destination">Optional destination for returned record headers.</param>
    /// <returns>Query result metadata.</returns>
    /// <exception cref="ZireaelException">Thrown when the native call fails.</exception>
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

    /// <summary>
    /// Queries debug records without returning headers, yielding only metadata.
    /// </summary>
    /// <param name="query">Query parameters.</param>
    /// <returns>Query result metadata.</returns>
    /// <exception cref="ZireaelException">Thrown when the native call fails.</exception>
    public ZrDebugQueryResult DebugQuery(in ZrDebugQuery query)
    {
        EnsureNotDisposed();

        unsafe
        {
            ThrowIfFailed(ZrNative.engine_debug_query(_handle, in query, null, 0, out var result));
            return result;
        }
    }

    /// <summary>
    /// Reads a debug record payload.
    /// </summary>
    /// <param name="recordId">Record identifier.</param>
    /// <param name="destination">Optional destination buffer for payload bytes.</param>
    /// <returns>Bytes written, or required size when no destination is provided.</returns>
    /// <exception cref="ZireaelException">Thrown when the native call fails.</exception>
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

    /// <summary>
    /// Reads aggregate debug statistics.
    /// </summary>
    /// <returns>Current debug stats.</returns>
    /// <exception cref="ZireaelException">Thrown when the native call fails.</exception>
    public ZrDebugStats GetDebugStats()
    {
        EnsureNotDisposed();
        ThrowIfFailed(ZrNative.engine_debug_get_stats(_handle, out var stats));
        return stats;
    }

    /// <summary>
    /// Exports the debug ring in binary form.
    /// </summary>
    /// <param name="destination">Optional destination buffer for export bytes.</param>
    /// <returns>Bytes written, or required size when no destination is provided.</returns>
    /// <exception cref="ZireaelException">Thrown when the native call fails.</exception>
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

    /// <summary>
    /// Clears all captured debug records.
    /// </summary>
    public void DebugReset()
    {
        EnsureNotDisposed();
        ZrNative.engine_debug_reset(_handle);
    }

    /// <summary>
    /// Releases the underlying native engine handle.
    /// </summary>
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