using JustLoaded.Content;

namespace JustLoaded.Filesystem;

public class CombinedFilesystem : IFilesystem {
    
    public bool HandlesSource => true;

    private readonly Dictionary<string, IFilesystem> _fileSystems = new();

    public void AddFileSystem(string name, IFilesystem filesystem) {
        _fileSystems.Add(name, filesystem);
    }
    
    Stream? IFilesystem.OpenFile(ModAssetPath path) {
        foreach (var fs in MatchModId(path.modSelector)) {
            var file = fs.Value.OpenFile(path);
            if (file != null) {
                return file;
            }    
        }
        return null;
    }

    public IEnumerable<ModAssetPath> ListFiles(ModAssetPath path, string pattern = "*", bool recursive = false) {
        foreach (var fs in MatchModId(path.modSelector)) {
            foreach (var file in fs.Value.ListFiles(path, pattern, recursive)) {
                yield return new ModAssetPath(fs.Key, file.path);
            }
        }
    }
    
    public IEnumerable<ModAssetPath> ListPaths(ModAssetPath path) {
        foreach (var fs in MatchModId(path.modSelector)) {
            foreach (var pathOut in fs.Value.ListPaths(path)) {
                yield return new ModAssetPath(fs.Key, pathOut.path);
            }
        }
    }

    public IEnumerable<KeyValuePair<string, IFilesystem>> MatchModId(string modId) {
        return _fileSystems.Where(pair => pair.Key.MatchPattern(modId));
    }
}