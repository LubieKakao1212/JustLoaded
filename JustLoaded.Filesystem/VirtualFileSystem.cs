using System.Text;
using JustLoaded.Util.Extensions;
using JustLoaded.Content;

namespace JustLoaded.Filesystem;

/// <summary>
/// 
/// </summary>
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

    public VirtualFilesystem() : this(DefaultIllegalCharacters) {
        
    }
    
    public VirtualFilesystem(params char[] illegalCharacters) {
        this._illegalCharacters = illegalCharacters;
    }
    
    public void AddFile(string filePath, byte[] data) {
        CallForPath(filePath, true,
            (system, file) => {
                system._files.Add(file, data);
                return 0;
            });
    }

    public void AddFile(string filePath, string content) {
        AddFile(filePath, Encoding.ASCII.GetBytes(content));
    }
    
    public bool HandlesSource => false;
    
    public Stream? OpenFile(string filePath) {
        return CallForPath(filePath, false,
            (system, file) => {
                if (!system._files.TryGetValue(file, out var content)) {
                    throw new FileNotFoundException();
                }

                return new MemoryStream(content);
            });
    }

    public IEnumerable<ContentKey> ListFiles(string path, string pattern = "*", bool recursive = false) {
        return CallForPath(path, false,
            (system, file) => {
                if (file != "") {
                    if (!system._directories.TryGetValue(file, out var s)) {
                        throw new DirectoryNotFoundException();
                    }
                    system = s;
                }
                
                var list = new List<string>();

                list.AddRange(system._files.Keys.Select((f) => CombinePath(path, f)));
                
                if (recursive) {
                    foreach (var directory in system._directories) {
                        list.AddRange(directory.Value.ListFiles("/", pattern, recursive).Select((key) => CombinePath(CombinePath(path, directory.Key), key.path)));
                    }
                }

                return list.Where((str) => MatchPattern(str, pattern)).Select((str) => new ContentKey("", str));
            });
    }

    public IEnumerable<ContentKey> ListPaths(string path) {
        return CallForPath(path, false,
            (system, file) => {
                if (file != "") {
                    if (!system._directories.TryGetValue(file, out var s)) {
                        throw new DirectoryNotFoundException();
                    }
                    system = s;
                }
                return system._directories.Keys.Select((str) => new ContentKey("", CombinePath(path, str)));
            });
    }

    private T CallForPath<T>(String path, bool createDirs, Func<VirtualFilesystem, string, T> action) {
        if (path.IndexOfAny(_illegalCharacters) >= 0) {
            throw new ArgumentException("Path contains invalid characters");
        }
        var i = path.IndexOf('/');
        
        //Skip '/' if its the first character
        if (i == 0) {
            path = path.Substring(1);
            i = path.IndexOf('/');
        }
        
        if (i < 0) {
            return action(this, path);
        }
        else {
            var p = path.Substring(0, i);
            if (_files.ContainsKey(p)) {
                throw new DirectoryNotFoundException("not a directory");
            }

            VirtualFilesystem vfs;
            
            if (createDirs) {
                vfs = _directories.GetValueOrSetDefaultLazy(p, () => new VirtualFilesystem(_illegalCharacters));
            }
            else if (_directories.TryGetValue(p, out var fs)) {
                vfs = fs;
            }
            else {
                throw new DirectoryNotFoundException("Not such file or directory:");
            }
            
            return vfs.CallForPath<T>(path.Substring(i + 1), createDirs, action);
        }
    }

    private bool MatchPattern(string str, string pattern) {
        var segments = pattern.Split('*');
        foreach (var p in segments) {
            var segment = p.Replace("*", "");
            var location = str.IndexOf(segment, StringComparison.Ordinal);
            if (location < 0) {
                return false;
            }
            str = str.Substring(location + segment.Length);
        }
        return true;
    }

    private string CombinePath(string path, string secondPath) {
        var s = 0;
        var l = path.Length;
        if (path != "" && path[^1] == '/') {
            path = path.Substring(s, --l);
        }

        if (path != "" && path[0] == '/') {
            path = path.Substring(++s, --l);
        }
        
        if (path == "") {
            return secondPath;
        }
        return path + "/" + secondPath;
    }
    
    
}