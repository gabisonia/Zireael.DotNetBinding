using System;
using System.Runtime.InteropServices;
using Zireael.Net.Internal;

namespace Zireael.Net.Interop;

internal static class ZrNative
{
    private const string LibraryName = "zireael";

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "zr_engine_config_default")]
    internal static extern ZrEngineConfig zr_engine_config_default();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "zr_engine_config_validate")]
    internal static extern ZrResult zr_engine_config_validate(in ZrEngineConfig cfg);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl,
        EntryPoint = "zr_engine_runtime_config_validate")]
    internal static extern ZrResult zr_engine_runtime_config_validate(in ZrEngineRuntimeConfig cfg);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "zr_limits_default")]
    internal static extern ZrLimits zr_limits_default();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "zr_limits_validate")]
    internal static extern ZrResult zr_limits_validate(in ZrLimits limits);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "zr_debug_config_default")]
    internal static extern ZrDebugConfig zr_debug_config_default();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_create")]
    internal static extern ZrResult engine_create(out IntPtr outEngine, in ZrEngineConfig cfg);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_destroy")]
    internal static extern void engine_destroy(IntPtr engine);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_poll_events")]
    internal static extern unsafe int engine_poll_events(ZrEngineSafeHandle engine, int timeoutMs, byte* outBuf,
        int outCap);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_post_user_event")]
    internal static extern unsafe ZrResult engine_post_user_event(ZrEngineSafeHandle engine, uint tag, byte* payload,
        int payloadLen);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_submit_drawlist")]
    internal static extern unsafe ZrResult engine_submit_drawlist(ZrEngineSafeHandle engine, byte* bytes, int bytesLen);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_present")]
    internal static extern ZrResult engine_present(ZrEngineSafeHandle engine);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_get_metrics")]
    internal static extern ZrResult engine_get_metrics(ZrEngineSafeHandle engine, ref ZrMetrics outMetrics);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_get_caps")]
    internal static extern ZrResult engine_get_caps(ZrEngineSafeHandle engine, out ZrTerminalCaps outCaps);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_get_terminal_profile")]
    internal static extern IntPtr engine_get_terminal_profile(ZrEngineSafeHandle engine);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_set_config")]
    internal static extern ZrResult engine_set_config(ZrEngineSafeHandle engine, in ZrEngineRuntimeConfig cfg);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_debug_enable")]
    internal static extern ZrResult engine_debug_enable(ZrEngineSafeHandle engine, in ZrDebugConfig config);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_debug_disable")]
    internal static extern void engine_debug_disable(ZrEngineSafeHandle engine);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_debug_query")]
    internal static extern unsafe ZrResult engine_debug_query(
        ZrEngineSafeHandle engine,
        in ZrDebugQuery query,
        ZrDebugRecordHeader* outHeaders,
        uint outHeadersCap,
        out ZrDebugQueryResult outResult);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_debug_get_payload")]
    internal static extern unsafe ZrResult engine_debug_get_payload(
        ZrEngineSafeHandle engine,
        ulong recordId,
        void* outPayload,
        uint outCap,
        out uint outSize);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_debug_get_stats")]
    internal static extern ZrResult engine_debug_get_stats(ZrEngineSafeHandle engine, out ZrDebugStats outStats);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_debug_export")]
    internal static extern unsafe int engine_debug_export(ZrEngineSafeHandle engine, byte* outBuf, nuint outCap);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "engine_debug_reset")]
    internal static extern void engine_debug_reset(ZrEngineSafeHandle engine);
}