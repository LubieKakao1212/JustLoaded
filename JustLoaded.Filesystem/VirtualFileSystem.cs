using System.Text;
using JustLoaded.Util.Extensions;
using JustLoaded.Content;
using PathLib;

namespace JustLoaded.Filesystem;

/// <remarks>Does not support moving up i.e. "../CoolDirectory/CoolFile"</remarks>
public class VirtualFilesystem : IFilesystem {

    private static readonly char[] DefaultIllegalCharacters = {
        '\\',
        '*',
        ' '
    };
    
    private readonly Dictionary<string, byte[]> _files = new();
    private readonly Dictionary<string, VirtualFilesystem> _directories = new();
    private readonly char[] _illegalCharacters;
    public bool HandlesSource => false;
    
    public VirtualFilesystem() : this(DefaultIllegalCharacters) {
        
    }
    
    public VirtualFilesystem(params char[] illegalCharacters) {
        this._illegalCharacters = illegalCharacters;
    }
    
    public void AddFile(IPurePath filePath, byte[] data) {
        CallForPath(filePath, true,
            (system, file) => {
                system._files.Add(file, data);
                return 0;
            });
    }

    public void AddFile(IPurePath filePath, string content) {
        AddFile(filePath, Encoding.ASCII.GetBytes(content));
    }
    
    public Stream? OpenFile(ModAssetPath filePath) {
        return CallForPath(filePath.path, false,
            (system, file) => {
                if (!system._files.TryGetValue(file, out var content)) {
                    throw new FileNotFoundException();
                }

                return new MemoryStream(content);
            });
    }

    public IEnumerable<ModAssetPath> ListFiles(ModAssetPath path, string pattern = "*", bool recursive = false) {
        var purePath = CutDot(path.path);
        
        var stringPath = purePath.ToPosix();
        return CallForPath(purePath, false,
            (system, file) => {
                if (file != "") {
                    if (!system._directories.TryGetValue(file, out var s)) {
                        throw new DirectoryNotFoundException();
                    }
                    system = s;
                }
                
                var list = new List<IPurePath>();

                list.AddRange(system._files.Keys.Select((f) => new PurePosixPath(stringPath, f)));
                
                if (recursive) {
                    foreach (var directory in system._directories) {
                        list.AddRange(directory.Value.ListFiles(new ModAssetPath("", new PurePosixPath("")), pattern, recursive).Select(p => path.path.Join(directory.Key.AsPath(), p.path)));
                    }
                }

                return list.Where(p => p.Filename.MatchPattern(pattern)).Select(p => new ModAssetPath("", p));
            });
    }
    
    public IEnumerable<ModAssetPath> ListPaths(ModAssetPath path) {
        var purePath = CutDot(path.path);
        return CallForPath(purePath, false,
            (system, file) => {
                if (file != "") {
                    if (!system._directories.TryGetValue(file, out var s)) {
                        throw new DirectoryNotFoundException();
                    }
                    system = s;
                }
                return system._directories.Keys.Select(str => new ModAssetPath("", purePath.Join(str)));
            });
    }

    private T CallForPath<T>(IPurePath path, bool createDirs, Func<VirtualFilesystem, string, T> action) {
        path = path.CollapseAbsolutePath();
        var stringPath = path.ToPosix();
        if (stringPath.IndexOfAny(_illegalCharacters) >= 0) {
            throw new ArgumentException("Path contains invalid characters");
        }

        var parts = new List<String>(path.Parts);

        if (parts[0].Equals(".")) {
            if (parts.Count == 1) {
                return action(this, "");
            }
            parts.RemoveAt(0);
        }
        
        if (!path.HasComponents(PathComponent.Dirname)) {
            return action(this, path.Filename);
        }
        else {
            var first = parts[0];
            if (_files.ContainsKey(first)) {
                throw new DirectoryNotFoundException("not a directory");
            }
            VirtualFilesystem vfs;
            
            if (createDirs) {
                vfs = _directories.GetValueOrSetDefaultLazy(first, () => new VirtualFilesystem(_illegalCharacters));
            }
            else if (_directories.TryGetValue(first, out var fs)) {
                vfs = fs;
            }
            else {
                throw new DirectoryNotFoundException($"Not such file or directory: {path.ToPosix()}");
            }
            
            return vfs.CallForPath<T>(new PurePosixPath(parts.Skip(1).ToArray()), createDirs, action);
        }
    }

    private IPurePath CutDot(IPurePath purePath) {
        var p = purePath.Parts.ToArray();
        if (p.Length > 1 && p[0] == ".") {
            purePath = purePath.RelativeTo(PathExtensions.LOCAL);
        }

        return purePath;
    }
    
    // private string CombinePath(string path, string secondPath) {
    //     var s = 0;
    //     var l = path.Length;
    //     if (path != "" && path[^1] == '/') {
    //         path = path.Substring(s, --l);
    //     }
    //
    //     if (path != "" && path[0] == '/') {
    //         path = path.Substring(++s, --l);
    //     }
    //     
    //     if (path == "") {
    //         return secondPath;
    //     }
    //     return path + "/" + secondPath;
    // }
    
}