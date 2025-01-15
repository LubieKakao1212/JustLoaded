using PathLib;

namespace JustLoaded.Filesystem;

public static class PathExtensions {
    
    public static readonly IPurePath Local = new PosixPath(".");
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

    public static IPurePath RelativeToFixed(this IPurePath path, IPurePath relativeTo) {
        var partsBase = new List<string>(path.Parts);
        var partsRelativeTo = new List<string>(relativeTo.Parts);
        //Capacity is upper limit
        var partsOut = new List<string>(partsBase.Count + partsRelativeTo.Count);
        
        int matchIdx = partsBase[0].Equals(".") ? 1 : 0;
        int baseCount = partsBase.Count;
        bool flag = matchIdx >= baseCount;

        int s = partsRelativeTo[0].Equals(".") ? 1 : 0;
        
        for (int i = s; i < partsRelativeTo.Count; i++) {
            if (!flag) {
                matchIdx++;
                if (i >= baseCount || !partsRelativeTo[i].Equals(partsBase[i])) {
                    flag = true;
                    matchIdx--;
                }
            }

            if (flag) {
                partsOut.Add("..");
            }
        }

        for (int i = matchIdx; i < partsBase.Count; i++) {
            partsOut.Add(partsBase[i]);
        }

        return new PosixPath(partsOut.ToArray());
    }
}