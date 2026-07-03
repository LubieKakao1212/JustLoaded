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
        return _nestedFilesystem.OpenFile(TransformKey(path));
    }

    public IEnumerable<IPurePath> ListFiles(IPurePath path, string pattern = "*", bool recursive = false)
    {
        return _nestedFilesystem.ListFiles(TransformKey(path), pattern, recursive)
            .Select(purePath => purePath.RelativeTo(_prefixPath));
    }

    public IEnumerable<IPurePath> ListPaths(IPurePath path)
    {
        return _nestedFilesystem.ListPaths(TransformKey(path))
            .Select(purePath => purePath.RelativeTo(_prefixPath));
    }

    private IPurePath TransformKey(in IPurePath key)
    {
        return TransformPath(key);
    }

    private IPurePath TransformPath(IPurePath path)
    {
        return _prefixPath.Join(path);
    }
}
