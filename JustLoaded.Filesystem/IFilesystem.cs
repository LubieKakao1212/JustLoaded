using PathLib;

namespace JustLoaded.Filesystem;

/// <summary>
///     Represents a read only file system
/// </summary>
public interface IFilesystem
{

    public bool HandlesSource { get; }

    public Stream? OpenFile(IPurePath path);

    public IEnumerable<IPurePath> ListFiles(IPurePath path, string pattern = "*", bool recursive = false);

    public IEnumerable<IPurePath> ListPaths(IPurePath path);

    public static void HandleSourceHandlingWarning(IFilesystem filesystem)
    {
        if (filesystem.HandlesSource)
        {
            //TODO use Logger (Warning)
            Console.Error.WriteLine("Nesting source-handling filesystems in one another will most likely cause issues");
        }
    }
}
