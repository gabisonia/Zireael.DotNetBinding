# Developing Zireael.Net

## Prerequisites

- .NET SDK (see `global.json`)
- CMake (for native Zireael build)

## Build managed projects

```bash
dotnet build Zireael.DotNetBinding.slnx
```

## Build native dependency

```bash
./eng/build-native.sh
```

## Run sample

```bash
./eng/run-sample.sh
```

## Pack NuGet

```bash
./eng/pack.sh
```

Package artifacts are emitted to `artifacts/`.

## CI

Use `.github/workflows/pack-multi-platform.yml` to build native runtime assets and pack one multi-platform NuGet package.
