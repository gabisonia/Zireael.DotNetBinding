using System.Buffers.Binary;
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

        var drawlist = BuildHelloDrawlistV1("Hello from Zireael.Net");
        var timer = Stopwatch.StartNew();
        while (timer.Elapsed < TimeSpan.FromSeconds(5))
        {
            var x = 2 + (renderedFrames % 28);
            SetDrawTextX(drawlist, x);
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
    Console.Error.WriteLine("If using local source native binaries, run ./eng/build-native.sh then ./eng/run-sample.sh.");
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

static byte[] BuildHelloDrawlistV1(string text)
{
    var textBytes = System.Text.Encoding.UTF8.GetBytes(text);
    const uint headerSize = 64;
    const uint cmdOff = headerSize;
    const uint cmdBytes = 56; // CLEAR(8) + DRAW_TEXT(48)
    const uint stringsSpanOff = 120;
    const uint stringsBytesOff = 128;

    var stringBytesLen = Align4((uint)textBytes.Length);
    var totalSize = stringsBytesOff + stringBytesLen;
    var bytes = new byte[totalSize];

    // Header
    WriteU32(bytes, 0, ZrDrawlistConstants.Magic);
    WriteU32(bytes, 4, ZrVersion.DrawlistVersionV1);
    WriteU32(bytes, 8, headerSize);
    WriteU32(bytes, 12, totalSize);
    WriteU32(bytes, 16, cmdOff);
    WriteU32(bytes, 20, cmdBytes);
    WriteU32(bytes, 24, 2); // command count
    WriteU32(bytes, 28, stringsSpanOff);
    WriteU32(bytes, 32, 1); // string count
    WriteU32(bytes, 36, stringsBytesOff);
    WriteU32(bytes, 40, stringBytesLen);

    // CLEAR cmd @ 64
    WriteU16(bytes, 64, (ushort)ZrDlOpcode.Clear);
    WriteU16(bytes, 66, 0);
    WriteU32(bytes, 68, 8);

    // DRAW_TEXT cmd @ 72
    WriteU16(bytes, 72, (ushort)ZrDlOpcode.DrawText);
    WriteU16(bytes, 74, 0);
    WriteU32(bytes, 76, 48);
    WriteI32(bytes, 80, 2); // x, animated per frame
    WriteI32(bytes, 84, 2); // y
    WriteU32(bytes, 88, 0); // string index
    WriteU32(bytes, 92, 0); // byte off
    WriteU32(bytes, 96, (uint)textBytes.Length);
    WriteU32(bytes, 100, 0x00FFFFFF); // fg
    WriteU32(bytes, 104, 0x00000000); // bg
    WriteU32(bytes, 108, 0); // attrs
    WriteU32(bytes, 112, 0);
    WriteU32(bytes, 116, 0);

    // Single string span @ 120
    WriteU32(bytes, 120, 0);
    WriteU32(bytes, 124, (uint)textBytes.Length);

    // String bytes @ 128
    Buffer.BlockCopy(textBytes, 0, bytes, 128, textBytes.Length);
    return bytes;
}

static void SetDrawTextX(byte[] drawlist, int x) => WriteI32(drawlist, 80, x);

static uint Align4(uint value) => (value + 3u) & ~3u;

static void WriteU16(byte[] dest, int offset, ushort value) =>
    BinaryPrimitives.WriteUInt16LittleEndian(dest.AsSpan(offset, 2), value);

static void WriteI32(byte[] dest, int offset, int value) =>
    BinaryPrimitives.WriteInt32LittleEndian(dest.AsSpan(offset, 4), value);

static void WriteU32(byte[] dest, int offset, uint value) =>
    BinaryPrimitives.WriteUInt32LittleEndian(dest.AsSpan(offset, 4), value);
