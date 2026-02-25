# Zireael.Net

.NET binding for the [Zireael](https://github.com/RtlZeroMemory/Zireael) terminal rendering engine.

## Prerequisites

- .NET SDK (`global.json`)
- CMake (to build native `zireael`)

## Build

```bash
dotnet build Zireael.DotNetBinding.slnx
```

## Build Native Library

```bash
./eng/build-native.sh
```

Native outputs:

- local run: `native/lib/`
- NuGet runtime assets: `native/runtimes/<rid>/native/`

## Pack NuGet

```bash
./eng/pack.sh
```

Artifacts:

- `artifacts/*.nupkg`
- `artifacts/*.snupkg`

## CI

Workflow: `.github/workflows/pack-multi-platform.yml`

It builds native binaries on Linux/macOS/Windows, stages runtime assets, and packs one multi-platform NuGet package.

## Docs

- `docs/DEVELOPING.md`
- `docs/NATIVE_PACKAGING.md`
