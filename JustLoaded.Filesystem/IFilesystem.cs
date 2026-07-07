using PathLib;

namespace JustLoaded.Filesystem;

/// <summary>
///     Represents a read only file system
/// </summary>
public interface IFilesystem
{
    public Stream? OpenFile(IPurePath path);

    public IEnumerable<IPurePath> ListFiles(IPurePath path, string pattern = "*", bool recursive = false);

    public IEnumerable<IPurePath> ListPaths(IPurePath path);
}
