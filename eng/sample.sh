#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

print_cmake_instructions() {
  echo "cmake is required but was not found in PATH." >&2
  echo "Install CMake, then rerun." >&2
  echo "  macOS:   brew install cmake" >&2
  echo "  Ubuntu:  sudo apt-get update && sudo apt-get install -y cmake" >&2
  echo "  Windows: choco install cmake --installargs 'ADD_CMAKE_TO_PATH=System' -y" >&2
}

if ! command -v dotnet >/dev/null 2>&1; then
  echo "dotnet SDK is required but was not found in PATH." >&2
  echo "Install .NET SDK (see global.json), then rerun." >&2
  exit 1
fi

if ! command -v cmake >/dev/null 2>&1; then
  print_cmake_instructions
  echo "After install, run: make sample (or ./eng/sample.sh)" >&2
  exit 1
fi

echo "[1/2] Building native zireael library..."
"${ROOT_DIR}/eng/build-native.sh"

echo "[2/2] Running Zireael sample..."
"${ROOT_DIR}/eng/run-sample.sh" "$@"
