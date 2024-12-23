using System.Diagnostics;
using JustLoaded.Content;
using JustLoaded.Util.Validation;
using PathLib;

namespace JustLoaded.Filesystem;

public class RelativeFilesystem : IFilesystem {

    public bool HandlesSource => _nestedFilesystem.HandlesSource;
    
    private readonly IFilesystem _nestedFilesystem;
    private readonly IPurePath _prefixPath;
    
    public RelativeFilesystem(IFilesystem nested, IPurePath prefixPath) {
        _nestedFilesystem = nested;
        _prefixPath = prefixPath;
        prefixPath.Matches(path => !path.IsAbsolute());
    }
    
    public Stream? OpenFile(ModAssetPath path) {
        return _nestedFilesystem.OpenFile(TransformKey(path));
    }

    public IEnumerable<ModAssetPath> ListFiles(ModAssetPath path, string pattern = "*", bool recursive = false) {
        return _nestedFilesystem.ListFiles(TransformKey(path), pattern, recursive)
            .Select(ModAssetPathExtensions.RelativeToSelect(_prefixPath));
    }

    public IEnumerable<ModAssetPath> ListPaths(ModAssetPath path) {
        return _nestedFilesystem.ListPaths(TransformKey(path))
            .Select(ModAssetPathExtensions.RelativeToSelect(_prefixPath));
    }

    private IPurePath TransformPath(IPurePath path) {
        return _prefixPath.Join(path);
    }

    private ModAssetPath TransformKey(in ModAssetPath key) {
        return new ModAssetPath(key.modSelector, TransformPath(key.path));
    }
    
}