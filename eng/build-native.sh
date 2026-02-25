#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
SOURCE_DIR="${1:-${ROOT_DIR}/upstream-zireael}"
BUILD_DIR="${ROOT_DIR}/native/build"
LEGACY_LIB_DIR="${ROOT_DIR}/native/lib"

if [[ ! -d "${SOURCE_DIR}" ]]; then
  echo "Source directory not found: ${SOURCE_DIR}" >&2
  exit 1
fi

if ! command -v cmake >/dev/null 2>&1; then
  echo "cmake is required but was not found in PATH." >&2
  echo "Install it (macOS: brew install cmake), then rerun this script." >&2
  exit 1
fi

detect_rid() {
  local os arch
  os="$(uname -s)"
  arch="$(uname -m)"

  case "${os}" in
    Darwin)
      case "${arch}" in
        arm64) echo "osx-arm64" ;;
        x86_64) echo "osx-x64" ;;
        *) return 1 ;;
      esac
      ;;
    Linux)
      case "${arch}" in
        x86_64) echo "linux-x64" ;;
        aarch64|arm64) echo "linux-arm64" ;;
        *) return 1 ;;
      esac
      ;;
    MINGW*|MSYS*|CYGWIN*|Windows_NT)
      case "${arch}" in
        x86_64|amd64) echo "win-x64" ;;
        arm64|aarch64) echo "win-arm64" ;;
        *) return 1 ;;
      esac
      ;;
    *)
      return 1
      ;;
  esac
}

RID="${ZIREAEL_RUNTIME_ID:-$(detect_rid || true)}"
if [[ -z "${RID}" ]]; then
  echo "Could not map host OS/arch to a .NET runtime identifier." >&2
  echo "Build completed binary can still be copied manually into native/runtimes/<rid>/native/." >&2
fi

cmake_args=(
  -DZIREAEL_BUILD_SHARED=ON
  -DZIREAEL_BUILD_EXAMPLES=OFF
  -DZIREAEL_BUILD_TESTS=OFF
)

if [[ -n "${ZIREAEL_CMAKE_EXTRA_ARGS:-}" ]]; then
  read -r -a extra_cmake_args <<< "${ZIREAEL_CMAKE_EXTRA_ARGS}"
  cmake_args+=("${extra_cmake_args[@]}")
fi

cmake -S "${SOURCE_DIR}" -B "${BUILD_DIR}" "${cmake_args[@]}"

cmake --build "${BUILD_DIR}" --config Release

mkdir -p "${LEGACY_LIB_DIR}"

copy_first_match() {
  local output_name="$1"
  shift
  local candidates=("$@")
  for candidate in "${candidates[@]}"; do
    if [[ -f "${candidate}" ]]; then
      cp "${candidate}" "${LEGACY_LIB_DIR}/${output_name}"
      echo "Copied ${candidate} -> ${LEGACY_LIB_DIR}/${output_name}"

      if [[ -n "${RID}" ]]; then
        local runtime_dir
        runtime_dir="${ROOT_DIR}/native/runtimes/${RID}/native"
        mkdir -p "${runtime_dir}"
        cp "${candidate}" "${runtime_dir}/${output_name}"
        echo "Copied ${candidate} -> ${runtime_dir}/${output_name}"
      fi

      return 0
    fi
  done

  echo "Could not find ${output_name} in build output." >&2
  return 1
}

case "$(uname -s)" in
  Darwin)
    copy_first_match "libzireael.dylib" \
      "${BUILD_DIR}/libzireael.dylib" \
      "${BUILD_DIR}/Release/libzireael.dylib"
    ;;
  Linux)
    copy_first_match "libzireael.so" \
      "${BUILD_DIR}/libzireael.so" \
      "${BUILD_DIR}/Release/libzireael.so"
    ;;
  MINGW*|MSYS*|CYGWIN*|Windows_NT)
    copy_first_match "zireael.dll" \
      "${BUILD_DIR}/zireael.dll" \
      "${BUILD_DIR}/Release/zireael.dll"
    ;;
  *)
    echo "Unsupported platform for this script. Build manually and stage into native/runtimes/<rid>/native/." >&2
    exit 1
    ;;
esac

echo
echo "Native library staged in ${LEGACY_LIB_DIR}."
if [[ -n "${RID}" ]]; then
  echo "NuGet runtime asset staged in native/runtimes/${RID}/native/."
fi
