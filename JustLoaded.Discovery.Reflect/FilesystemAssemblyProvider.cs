using System.Reflection;
using JustLoaded.Core.Discovery;
using JustLoaded.Filesystem;

namespace JustLoaded.Discovery.Reflect;

public class FilesystemAssemblyProvider : IAssemblyProvider {

    private readonly IFilesystem _filesystem;

    public FilesystemAssemblyProvider(IFilesystem filesystem) {
        this._filesystem = filesystem;
    }
    
    public IEnumerable<Assembly> GetAssemblies() {
        foreach (var file in _filesystem.ListFiles(".".AsPath().FromAnyMod(), "*.dll", true)) {
            using var stream = _filesystem.OpenFile(file);
            using var memStream = new MemoryStream();
            stream!.CopyTo(memStream);
            yield return Assembly.Load(memStream.ToArray());
        }
    }
}