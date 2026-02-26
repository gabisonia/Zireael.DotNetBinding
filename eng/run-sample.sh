#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PROJECT="${ROOT_DIR}/samples/Zireael.Net.Sample/Zireael.Net.Sample.csproj"

OS_NAME="$(uname -s)"

if [[ "${OS_NAME}" == "Darwin" ]]; then
  LIB_FILE="${ROOT_DIR}/native/lib/libzireael.dylib"
  if [[ ! -f "${LIB_FILE}" ]]; then
    echo "Native library not found: ${LIB_FILE}" >&2
    echo "Run 'make sample' (or './eng/build-native.sh') first." >&2
    exit 1
  fi

  DYLD_LIBRARY_PATH="${ROOT_DIR}/native/lib:${DYLD_LIBRARY_PATH:-}" \
    dotnet run --project "${PROJECT}" "$@"
  exit 0
fi

if [[ "${OS_NAME}" == "Linux" ]]; then
  LIB_FILE="${ROOT_DIR}/native/lib/libzireael.so"
  if [[ ! -f "${LIB_FILE}" ]]; then
    echo "Native library not found: ${LIB_FILE}" >&2
    echo "Run 'make sample' (or './eng/build-native.sh') first." >&2
    exit 1
  fi

  LD_LIBRARY_PATH="${ROOT_DIR}/native/lib:${LD_LIBRARY_PATH:-}" \
    dotnet run --project "${PROJECT}" "$@"
  exit 0
fi

echo "Unsupported platform for run-sample.sh" >&2
exit 1
