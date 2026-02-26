# Zireael.Net

`Zireael.Net` is a .NET binding for the [Zireael](https://github.com/RtlZeroMemory/Zireael) C engine.

## Includes

- ABI-accurate C# enums and structs for `include/zr/*.h`
- `P/Invoke` bindings for engine/config/metrics/caps/debug APIs
- Managed `ZireaelEngine` wrapper with `SafeHandle`
- `ZrDrawlistBuilder` for safe drawlist byte construction (v1)
- `ZrEventBatchReader` for safe packed event-batch parsing (v1)

## Ergonomic APIs

### Build drawlists without manual offsets

```csharp
var style = new ZrDlStyle
{
    Fg = 0x00FFFFFF,
    Bg = 0x00000000,
    Attrs = 0,
    Reserved0 = 0
};

var builder = new ZrDrawlistBuilder();
builder.Clear();
builder.DrawText(2, 2, "Hello", in style);
var drawlist = builder.Build();

engine.SubmitDrawlist(drawlist);
engine.Present();
```

### Parse event batches safely

```csharp
var written = engine.PollEvents(timeoutMs, buffer);
if (written > 0)
{
    var batch = new byte[written];
    Buffer.BlockCopy(buffer, 0, batch, 0, written);

    if (ZrEventBatchReader.TryCreate(batch, out var reader))
    {
        foreach (var record in reader!.EnumerateRecords())
        {
            if (record.TryGetKey(out var key))
            {
                // Handle key event
            }
        }
    }
}
```

## Native Dependency

The package requires the native `zireael` shared library.

- macOS: `libzireael.dylib`
- Linux: `libzireael.so`
- Windows: `zireael.dll`

If runtime assets are not bundled in the package used by your app, provide the library via standard loader paths (`PATH`, `LD_LIBRARY_PATH`, `DYLD_LIBRARY_PATH`) or next to the app executable.
