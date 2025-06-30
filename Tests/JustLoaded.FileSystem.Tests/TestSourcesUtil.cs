using JustLoaded.Filesystem;

namespace JustLoaded.FileSystem.Tests;

public static class TestSourcesUtil {

    public static ModAssetPath BlankMod(this string path) {
        return path.AsPath().FromMod("");
    }

    public static IEnumerable<ModAssetPath> PathsFromCommonMod(string mod, params string[] files) {
        return files.Select(s => s.AsPath().FromMod(mod));
    }
    
    
}