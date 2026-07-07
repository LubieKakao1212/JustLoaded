using JustLoaded.Util.Validation;
using PathLib;

namespace JustLoaded.Filesystem;

public class RelativeFilesystem : IFilesystem
{

    private readonly IFilesystem _nestedFilesystem;
    private readonly IPurePath _prefixPath;

    public RelativeFilesystem(IFilesystem nested, IPurePath prefixPath)
    {
        _nestedFilesystem = nested;
        _prefixPath = prefixPath;
        prefixPath.Matches(path => !path.IsAbsolute());
    }

    public bool HandlesSource => _nestedFilesystem.HandlesSource;

    public Stream? OpenFile(IPurePath path)
    {
        return _nestedFilesystem.OpenFile(TransformPath(path));
    }

    public IEnumerable<IPurePath> ListFiles(IPurePath path, string pattern = "*", bool recursive = false)
    {
        return _nestedFilesystem.ListFiles(TransformPath(path), pattern, recursive)
            .Select(purePath => purePath.RelativeTo(_prefixPath));
    }

    public IEnumerable<IPurePath> ListPaths(IPurePath path)
    {
        return _nestedFilesystem.ListPaths(TransformPath(path))
            .Select(purePath => purePath.RelativeTo(_prefixPath));
    }

    private IPurePath TransformPath(IPurePath path)
    {
        return _prefixPath.Join(path);
    }
}
