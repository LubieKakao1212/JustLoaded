using System.Security.AccessControl;
using JustLoaded.Content;

namespace JustLoaded.Filesystem;

[Obsolete("Experimental)")]
public class RelativeFilesystem : IFilesystem {

    public bool HandlesSource => _nestedFilesystem.HandlesSource;
    
    private readonly IFilesystem _nestedFilesystem;
    private readonly string _pathPrefix;
    
    public RelativeFilesystem(IFilesystem nested, string pathPrefix) {
        this._nestedFilesystem = nested;
        this._pathPrefix = pathPrefix;
    }

    Stream? IFilesystem.OpenFile(string path) {
        return _nestedFilesystem.OpenFile(TransformPath(path));
    }
    
    public Stream? OpenFile(ContentKey path) {
        return _nestedFilesystem.OpenFile(TransformKey(path));
    }

    public IEnumerable<ContentKey> ListFiles(string path, string pattern = "*", bool recursive = false) {
        return _nestedFilesystem.ListFiles(TransformPath(path), pattern, recursive);
    }

    public IEnumerable<ContentKey> ListFiles(ContentKey path, string pattern = "*", bool recursive = false) {
        return _nestedFilesystem.ListFiles(TransformKey(path), pattern, recursive);
    }

    public IEnumerable<ContentKey> ListPaths(string path) {
        return _nestedFilesystem.ListPaths(TransformPath(path));
    }

    public IEnumerable<ContentKey> ListPaths(ContentKey path) {
        return _nestedFilesystem.ListPaths(TransformKey(path));
    }

    private string TransformPath(string path) {
        return Path.Combine(_pathPrefix, path);
    }

    private ContentKey TransformKey(in ContentKey key) {
        return new ContentKey(key.source, TransformPath(key.path));
    }
    
}