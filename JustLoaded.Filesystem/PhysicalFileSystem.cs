using PathLib;

namespace JustLoaded.Filesystem;

/// <summary>
///     Issue: Does NOT work with absolute paths
/// </summary>
public class PhysicalFilesystem : IFilesystem
{

    /// <summary>
    /// </summary>
    /// <param name="root">Issue: MUST be a relative path</param>
    public PhysicalFilesystem(IPurePath root)
    {
        Root = new PosixPath(root.ToPosix());
    }
    private IPath Root { get; }

    public bool HandlesSource => false;

    public Stream? OpenFile(IPurePath path)
    {
        var concretePath = ApplyPath(path);
        if (concretePath.IsDir())
        {
            Console.WriteLine("Requested file is a directory");
            return null;
        }
        if (!concretePath.IsFile())
        {
            return null;
        }
        return concretePath.Open(FileMode.Open);
    }

    public IEnumerable<IPurePath> ListFiles(IPurePath path, string pattern = "*", bool recursive = false)
    {
        var concretePath = ApplyPath(path);
        if (!concretePath.IsDir())
        {
            return Enumerable.Empty<IPurePath>();
        }

        return concretePath.ListDir(recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
            .Where(path1 => path1.IsFile())
            .Where(path1 => path1.Filename.MatchPattern(pattern))
            .Select(RestorePath);
    }

    public IEnumerable<IPurePath> ListPaths(IPurePath path)
    {
        var concretePath = ApplyPath(path);
        if (!concretePath.IsDir())
        {
            return [];
        }

        return concretePath.ListDir(SearchOption.TopDirectoryOnly)
            .Where(path1 => path1.IsDir())
            .Select(RestorePath);
    }

    private IPath ApplyPath(IPurePath path)
    {
        return Root.Join(path.CollapseAbsolutePath());
    }

    private IPurePath RestorePath(IPath path)
    {
        return path.RelativeToFixed(PathExtensions.CurrentDirectory).RelativeToFixed(Root);
    }
}
