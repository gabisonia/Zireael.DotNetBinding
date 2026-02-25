# Zireael.Net

[![NuGet version](https://img.shields.io/nuget/vpre/Zireael.Net.svg)](https://www.nuget.org/packages/Zireael.Net)
[![pack-multi-platform](https://github.com/gabisonia/Zireael.DotNetBinding/actions/workflows/pack-multi-platform.yml/badge.svg)](https://github.com/gabisonia/Zireael.DotNetBinding/actions/workflows/pack-multi-platform.yml)
[![License: Apache-2.0](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

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
