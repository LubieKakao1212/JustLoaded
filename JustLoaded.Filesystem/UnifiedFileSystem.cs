namespace JustLoaded.Filesystem;

//TODO write tests
public class UnifiedFileSystem : IFilesystem {

    private readonly List<IFilesystem> _filesystems;
   
    public bool HandlesSource { get; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filesystems">First has more priority than last</param>
    /// <exception cref="ApplicationException"></exception>
    public UnifiedFileSystem(IEnumerable<IFilesystem> filesystems) {
        _filesystems = new List<IFilesystem>(filesystems);
        
        HandlesSource = _filesystems.Aggregate((bool?) null, (v, fs) => {
            if (v.HasValue && !fs.HandlesSource) {
                throw new ApplicationException($"Cannot mix source-handling and non-source-handling filesystems under {nameof(UnifiedFileSystem)}");
            }
            return (v = fs.HandlesSource);
        })!.Value;
    }
    
    public Stream? OpenFile(ModAssetPath path) {
        foreach (var fs in _filesystems) {
            var file = fs.OpenFile(path);
            if (file != null) {
                return file;
            }
        }
        return null;
    }

    public IEnumerable<ModAssetPath> ListFiles(ModAssetPath path, string pattern = "*", bool recursive = false) {
        var visited = new HashSet<ModAssetPath>();

        foreach (var fs in _filesystems) {
            foreach (var file in fs.ListFiles(path, pattern, recursive)) {
                if (visited.Add(file)) {
                    yield return file;
                }
            }
        }
    }

    public IEnumerable<ModAssetPath> ListPaths(ModAssetPath path) {
        var visited = new HashSet<ModAssetPath>();

        foreach (var fs in _filesystems) {
            foreach (var path1 in fs.ListPaths(path)) {
                if (visited.Add(path1)) {
                    yield return path1;
                }
            }
        }
    }
}