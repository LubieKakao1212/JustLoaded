using System.Runtime.InteropServices;
using PathLib;

namespace JustLoaded.Filesystem;

public class ModAssetPath {

    public static readonly ModAssetPath Empty = new("*", "".AsPath());

    public readonly string modSelector;
    public readonly IPurePath path;

    /*
    internal ModAssetPath(string modSelector, string path) {
        this.modSelector = modSelector;
        this.path = path.AsPath();
    }
    */

    internal ModAssetPath(string modSelector, IPurePath path) {
        this.modSelector = modSelector;
        this.path = path;
    }

    public ModAssetPath WithModSelector(string modSelector) {
        return new ModAssetPath(modSelector, new PosixPath(path.ToPosix()));
    }
    
    public bool Equals(ModAssetPath other) {
        return modSelector == other.modSelector && path.ToPosix() == other.path.ToPosix();
    }

    public override bool Equals(object? obj) {
        return obj is ModAssetPath other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(modSelector, path.ToPosix());
    }

    public override string ToString() {
        return $"{modSelector}:{path.ToPosix()}";
    }
}