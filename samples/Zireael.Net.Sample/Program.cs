using System.Diagnostics;
using Zireael.Net;

Console.WriteLine("Zireael sample");

try
{
    var cfg = ZireaelEngine.CreatePinnedDefaultConfig(ZrVersion.DrawlistVersionV1);
    if (ZireaelEngine.ValidateConfig(in cfg) != ZrResult.Ok)
    {
        Console.Error.WriteLine("Unable to start renderer with the default configuration.");
        return 2;
    }

    ZrTerminalCaps caps;
    string terminalVersion;
    ZrMetrics metrics;
    var renderedFrames = 0;

    using (var engine = ZireaelEngine.Create(in cfg))
    {
        caps = engine.GetCapabilities();
        var profile = engine.GetTerminalProfile();
        terminalVersion = profile.GetVersionString();
        var style = new ZrDlStyle
        {
            Fg = 0x00FFFFFF,
            Bg = 0x00000000,
            Attrs = 0,
            Reserved0 = 0
        };
        var builder = new ZrDrawlistBuilder();

        var timer = Stopwatch.StartNew();
        while (timer.Elapsed < TimeSpan.FromSeconds(5))
        {
            var x = 2 + (renderedFrames % 28);
            builder.Reset();
            builder.Clear();
            builder.DrawText(x, 2, "Hello from Zireael.Net", in style);
            var drawlist = builder.Build();
            engine.SubmitDrawlist(drawlist);
            engine.Present();

            renderedFrames++;
            Thread.Sleep(50);
        }

        metrics = engine.GetMetrics();
    }

    Console.WriteLine($"Rendered frames: {renderedFrames}");
    Console.WriteLine($"Terminal Id: {caps.TerminalId}");
    Console.WriteLine($"Terminal version: {terminalVersion}");
    Console.WriteLine($"Color mode: {caps.ColorMode}");
    Console.WriteLine($"Capability flags: {caps.CapFlags}");
    Console.WriteLine($"Negotiated drawlist version: {metrics.NegotiatedDrawlistVersion}");
    Console.WriteLine("Sample completed.");
    return 0;
}
catch (DllNotFoundException ex)
{
    Console.Error.WriteLine("Could not load the native Zireael runtime on this machine.");
    Console.Error.WriteLine(
        "If using local source native binaries, run ./eng/build-native.sh then ./eng/run-sample.sh.");
    Console.Error.WriteLine("If using NuGet package mode, ensure the package includes your platform runtime asset.");
    Console.Error.WriteLine(ex.Message);
    return 3;
}
catch (ZireaelException ex)
{
    Console.Error.WriteLine("Zireael failed to initialize.");
    Console.Error.WriteLine($"{ex.Result} ({(int)ex.Result})");
    return 4;
}
