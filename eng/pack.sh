#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PROJECT="${ROOT_DIR}/src/Zireael.Net/Zireael.Net.csproj"
OUTPUT_DIR="${ROOT_DIR}/artifacts"
RUNTIME_ROOT="${ROOT_DIR}/native/runtimes"

mkdir -p "${OUTPUT_DIR}"

if [[ "${ZIREAEL_ALLOW_EMPTY_RUNTIME_ASSETS:-0}" != "1" ]]; then
  runtime_asset_count="$(find "${RUNTIME_ROOT}" -type f \
    \( -name '*.so' -o -name '*.dylib' -o -name '*.dll' \) 2>/dev/null | wc -l | tr -d ' ')"
  if [[ "${runtime_asset_count}" == "0" ]]; then
    echo "No native runtime assets found under ${RUNTIME_ROOT}." >&2
    echo "Build/stage native binaries first (for example via CI matrix or ./eng/build-native.sh)." >&2
    echo "Set ZIREAEL_ALLOW_EMPTY_RUNTIME_ASSETS=1 to bypass this guard." >&2
    exit 1
  fi
fi

DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 \
  dotnet pack "${PROJECT}" \
    -c Release \
    -o "${OUTPUT_DIR}" \
    --nologo \
    -p:IncludeSymbols=true \
    -p:SymbolPackageFormat=snupkg \
    "$@"
