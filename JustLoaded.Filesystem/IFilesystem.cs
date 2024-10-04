using JustLoaded.Content;

namespace JustLoaded.Filesystem;

/// <summary>
/// Represents a read only file system
/// </summary>
public interface IFilesystem {

    public bool HandlesSource { get; }

    public Stream? OpenFile(ModAssetPath path);

    public IEnumerable<ModAssetPath> ListFiles(ModAssetPath path, string pattern = "*", bool recursive = false);

    public IEnumerable<ModAssetPath> ListPaths(ModAssetPath path);
    
    public static void HandleSourceHandlingWarning(IFilesystem filesystem) {
        if (filesystem.HandlesSource) {
            //TODO use Logger (Warning)
            Console.Error.WriteLine("Nesting source-handling filesystems in one another will most likely cause issues");
        }
    }
    
}