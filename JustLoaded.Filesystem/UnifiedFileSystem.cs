using PathLib;

namespace JustLoaded.Filesystem;

//TODO write tests
public class UnifiedFileSystem : IFilesystem
{

    private readonly List<IFilesystem> _filesystems;

    /// <summary>
    /// </summary>
    /// <param name="filesystems">First has more priority than last</param>
    /// <exception cref="ApplicationException"></exception>
    public UnifiedFileSystem(IEnumerable<IFilesystem> filesystems)
    {
        _filesystems = new List<IFilesystem>(filesystems);

        HandlesSource = _filesystems.Aggregate((bool?)null, (v, fs) =>
        {
            if (v.HasValue && !fs.HandlesSource)
            {
                throw new ApplicationException($"Cannot mix source-handling and non-source-handling filesystems under {nameof(UnifiedFileSystem)}");
            }
            return fs.HandlesSource;
        })!.Value;
    }

    public bool HandlesSource { get; }

    public Stream? OpenFile(IPurePath path)
    {
        return _filesystems.Select(fs => fs.OpenFile(path)).OfType<Stream>().FirstOrDefault();
    }

    public IEnumerable<IPurePath> ListFiles(IPurePath path, string pattern = "*", bool recursive = false)
    {
        var visited = new HashSet<IPurePath>();

        foreach (var fs in _filesystems)
        {
            foreach (var file in fs.ListFiles(path, pattern, recursive))
            {
                if (visited.Add(file))
                {
                    yield return file;
                }
            }
        }
    }

    public IEnumerable<IPurePath> ListPaths(IPurePath path)
    {
        var visited = new HashSet<IPurePath>();

        foreach (var fs in _filesystems)
        {
            foreach (var path1 in fs.ListPaths(path))
            {
                if (visited.Add(path1))
                {
                    yield return path1;
                }
            }
        }
    }
}
