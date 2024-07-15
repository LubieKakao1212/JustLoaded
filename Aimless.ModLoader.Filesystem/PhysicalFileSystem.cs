using Aimless.ModLoader.Content;

namespace Aimless.ModLoader.Filesystem;

public class PhysicalFilesystem : IFilesystem {

    public bool HandlesSource => false;

    public string Root { get; private set; }
    
    public PhysicalFilesystem(string root) {
        this.Root = Path.GetFullPath(root);
    }
    
    Stream? IFilesystem.OpenFile(string path) {
        try {
            return File.OpenRead(ApplyPath(path));
        }
        catch (DirectoryNotFoundException e) {
            //TODO use Logger
            Console.WriteLine(e);
        }
        catch (FileNotFoundException e) {
            //TODO use Logger
            Console.WriteLine(e);
        }

        return null;
    }

    public IEnumerable<ContentKey> ListFiles(string path, string pattern = "*", bool recursive = false) {
        try {
            return Directory.EnumerateFiles(ApplyPath(Root), pattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Select((p) => new ContentKey("", GetStandardPath(p)));
        }
        catch (DirectoryNotFoundException e) {
            //TODO use Logger
            Console.WriteLine(e);
        }

        return Enumerable.Empty<ContentKey>();
    }

    public IEnumerable<ContentKey> ListPaths(string path) {
        try {
            return Directory.EnumerateDirectories(ApplyPath(path), "*").Select((p) => new ContentKey("", GetStandardPath(p)));
        }
        catch (DirectoryNotFoundException e) {
            //TODO use Logger
            Console.WriteLine(e);
        }

        return Enumerable.Empty<ContentKey>();
    }

    private string ApplyPath(string path) {
        return Path.Combine(Root, path);
    }

    private string GetStandardPath(string path) {
        return Path.GetRelativePath(Root, path).Replace('\\', '/');
    }
    
    
}