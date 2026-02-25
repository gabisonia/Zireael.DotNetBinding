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

What it does:

1. Builds native binaries for:
   - `linux-x64`
   - `osx-x64`
   - `osx-arm64`
   - `win-x64`
2. Stages runtime assets under `native/runtimes/<rid>/native/`
3. Packs one multi-platform NuGet package
4. On `v*` tags, publishes packages automatically

Manual run: trigger with `workflow_dispatch`.

## Docs

- `docs/DEVELOPING.md`
- `docs/NATIVE_PACKAGING.md`
