using System.Text;
using Zireael.Net;

Console.WriteLine("Zireael sample");

try
{
    var cfg = ZireaelEngine.CreatePinnedDefaultConfig();
    if (ZireaelEngine.ValidateConfig(in cfg) != ZrResult.Ok)
    {
        Console.Error.WriteLine("Unable to start renderer with the default configuration.");
        return 2;
    }

    using var engine = ZireaelEngine.Create(in cfg);

    var caps = engine.GetCapabilities();
    var profile = engine.GetTerminalProfile();
    var metrics = engine.GetMetrics();

    Console.WriteLine($"Terminal Id: {caps.TerminalId}");
    Console.WriteLine($"Terminal version: {profile.GetVersionString()}");
    Console.WriteLine($"Color mode: {caps.ColorMode}");
    Console.WriteLine($"Capability flags: {caps.CapFlags}");
    Console.WriteLine($"Negotiated drawlist version: {metrics.NegotiatedDrawlistVersion}");

    var payload = Encoding.UTF8.GetBytes("hello-from-zireael-dotnet");
    engine.PostUserEvent(42, payload);

    var eventBuffer = new byte[16 * 1024];
    var bytes = engine.PollEvents(50, eventBuffer);
    Console.WriteLine($"Polled event bytes: {bytes}");

    return 0;
}
catch (DllNotFoundException ex)
{
    Console.Error.WriteLine("Could not load the native Zireael runtime on this machine.");
    Console.Error.WriteLine(ex.Message);
    return 3;
}
catch (ZireaelException ex)
{
    Console.Error.WriteLine("Zireael failed to initialize.");
    Console.Error.WriteLine($"{ex.Result} ({(int)ex.Result})");
    return 4;
}