using JustLoaded.Content;

namespace JustLoaded.Filesystem;

[Obsolete("Experimental)")]
public class CombinedFilesystem : IFilesystem {
    
    public bool HandlesSource => true;

    private Dictionary<string, IFilesystem> _fileSystems = new();

    public void AddFileSystem(string name, IFilesystem filesystem) {
        _fileSystems.Add(name, filesystem);
    }
    
    Stream? IFilesystem.OpenFile(string path) {
        foreach (var fs in _fileSystems.Values) {
            var file = fs.OpenFile(path);
            if (file != null) {
                return file;
            }
        }

        return null;
    }

    public Stream? OpenFile(ContentKey path) {
        var fs = _fileSystems[path.source];
        return fs.OpenFile(path);
    }

    public IEnumerable<ContentKey> ListFiles(string path, string pattern = "*", bool recursive = false) {
        foreach (var fs in _fileSystems) {
            foreach (var file in fs.Value.ListFiles(path, pattern, recursive)) {
                yield return new ContentKey(fs.Key, file.path);
            }
        }
    }

    public IEnumerable<ContentKey> ListFiles(ContentKey path, string pattern = "*", bool recursive = false) {
        var fs = _fileSystems[path.source];
        return fs.ListFiles(path, pattern, recursive).Select((key) => new ContentKey(path.source, key.path));
    }

    public IEnumerable<ContentKey> ListPaths(string path) {
        foreach (var fs in _fileSystems) {
            foreach (var pathOut in fs.Value.ListPaths(path)) {
                yield return new ContentKey(fs.Key, pathOut.path);
            }
        }
    }
    
    public IEnumerable<ContentKey> ListPaths(ContentKey path) {
        var fs = _fileSystems[path.source];
        return fs.ListPaths(path).Select((key) => new ContentKey(path.source, key.path));
    }
    
}