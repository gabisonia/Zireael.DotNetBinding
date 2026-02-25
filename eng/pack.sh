#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PROJECT="${ROOT_DIR}/src/Zireael.Net/Zireael.Net.csproj"
OUTPUT_DIR="${ROOT_DIR}/artifacts"

mkdir -p "${OUTPUT_DIR}"

DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 \
  dotnet pack "${PROJECT}" \
    -c Release \
    -o "${OUTPUT_DIR}" \
    --nologo \
    -p:IncludeSymbols=true \
    -p:SymbolPackageFormat=snupkg \
    "$@"
