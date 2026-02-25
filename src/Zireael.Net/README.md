# Zireael.Net

`Zireael.Net` is a .NET binding for the [Zireael](https://github.com/RtlZeroMemory/Zireael) C engine.

## Includes

- ABI-accurate C# enums and structs for `include/zr/*.h`
- `P/Invoke` bindings for engine/config/metrics/caps/debug APIs
- Managed `ZireaelEngine` wrapper with `SafeHandle`

## Native Dependency

The package requires the native `zireael` shared library.

- macOS: `libzireael.dylib`
- Linux: `libzireael.so`
- Windows: `zireael.dll`

If runtime assets are not bundled in the package used by your app, provide the library via standard loader paths (`PATH`, `LD_LIBRARY_PATH`, `DYLD_LIBRARY_PATH`) or next to the app executable.
