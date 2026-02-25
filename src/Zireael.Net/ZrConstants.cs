namespace Zireael.Net;

public static class ZrEventConstants
{
    public const uint Magic = 0x5645525A;
    public const uint BatchTruncated = 1u << 0;
}

public static class ZrDebugConstants
{
    public const int SourceFileLength = 32;
    public const int MessageLength = 64;
}

public static class ZrTerminalConstants
{
    public const int VersionLength = 64;
}

public static class ZrDrawlistConstants
{
    // Defined in docs/abi/drawlist-format.md as little-endian bytes for "ZRDL".
    public const uint Magic = 0x4C44525A;
}