using PathLib;

namespace JustLoaded.Filesystem;

public static class PathExtensions {
    
    public static readonly IPurePath LOCAL = new PosixPath(".");
    public static readonly IPurePath CurrentDirectory = new PurePosixPath(Paths.CurrentDirectory.ToPosix());

    public static IPurePath AsPath(this string raw) {
        return new PosixPath(raw);
    }

    public static ModAssetPath FromMod(this IPurePath path, string modSelector) {
        return new ModAssetPath(modSelector, path);
    }

    public static ModAssetPath FromAnyMod(this IPurePath path) {
        return new ModAssetPath("*", path);
    }
    
}