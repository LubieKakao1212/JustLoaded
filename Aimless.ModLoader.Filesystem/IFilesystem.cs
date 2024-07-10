using Aimless.ModLoader.Content;

namespace Aimless.ModLoader.Filesystem;

/// <summary>
/// Represents a read only file system
/// </summary>
public interface IFilesystem {

    public bool HandlesSource { get; }

    protected internal Stream? OpenFile(string path);

    public Stream? OpenFile(ContentKey path) {
        return OpenFile(path.path);
    }

    public IEnumerable<ContentKey> ListFiles(string path, string pattern = "*", bool recursive = false);
    public IEnumerable<ContentKey> ListFiles(ContentKey path, string pattern = "*", bool recursive = false) {
        return ListFiles(path.path, pattern, recursive);
    }
    
    public IEnumerable<ContentKey> ListPaths(string path);
    public IEnumerable<ContentKey> ListPaths(ContentKey path) {
        return ListPaths(path.path);
    }

    public static void HandleSourceHandlingWarning(IFilesystem filesystem) {
        if (filesystem.HandlesSource) {
            //TODO use Logger (Warning)
            Console.Error.WriteLine("Nesting source-handling filesystems in one another will most likely cause issues");
        }
    }
    
}