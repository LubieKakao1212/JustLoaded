using System.Runtime.InteropServices;
using JustLoaded.Content;
using PathLib;

namespace JustLoaded.Filesystem;

public class PhysicalFilesystem : IFilesystem {
    
    public bool HandlesSource => false;
    public IPath Root { get; }
    
    public PhysicalFilesystem(IPurePath root) {
        Root = new PosixPath(root.ToPosix());
    }

    public Stream? OpenFile(ModAssetPath path) {
        var concretePath = ApplyPath(path.path);
        return concretePath.Open(FileMode.Open);
    }

    public IEnumerable<ModAssetPath> ListFiles(ModAssetPath path, string pattern = "*", bool recursive = false) {
        var concretePath = ApplyPath(path.path);
        return concretePath.ListDir(recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
            .Where(path1 => path1.IsFile())
            .Where(path1 => path1.Filename.MatchPattern(pattern))
            .Select(path1 => new ModAssetPath("", RestorePath(path1)));
    }

    public IEnumerable<ModAssetPath> ListPaths(ModAssetPath path) {
        var concretePath = ApplyPath(path.path);
        return concretePath.ListDir(SearchOption.TopDirectoryOnly)
            .Where(path1 => path1.IsDir())
            .Select(path1 => new ModAssetPath("", RestorePath(path1)));
    }

    
    private IPath ApplyPath(IPurePath path) {
        return Root.Join(path.CollapseAbsolutePath());
    }

    private IPurePath RestorePath(IPath path) {
        return path.RelativeTo(PathExtensions.CurrentDirectory).RelativeTo(Root);
    }
}