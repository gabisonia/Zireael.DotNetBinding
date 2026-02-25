# Native Packaging Model

`Zireael.Net` ships managed code plus optional native runtime assets in one package.

## Runtime asset layout

Place native binaries in this layout before running `./eng/pack.sh`:

- `native/runtimes/osx-x64/native/libzireael.dylib`
- `native/runtimes/osx-arm64/native/libzireael.dylib`
- `native/runtimes/linux-x64/native/libzireael.so`
- `native/runtimes/linux-arm64/native/libzireael.so`
- `native/runtimes/win-x64/native/zireael.dll`
- `native/runtimes/win-arm64/native/zireael.dll`

Any files present under `native/runtimes/**/native/` are packed into NuGet under `runtimes/<rid>/native/`.

## Host build helper

`./eng/build-native.sh` builds host-native binaries and stages them into:

- `native/lib/` (legacy local run path)
- `native/runtimes/<host-rid>/native/` (NuGet runtime asset path)

## CI recommendation

Use matrix builds per RID to produce native binaries, stage them in `native/runtimes/<rid>/native/`,
then run a final pack step to produce the multi-platform package.

This repository already includes `.github/workflows/pack-multi-platform.yml` implementing that flow.
