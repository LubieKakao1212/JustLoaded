using System.Reflection;
using Aimless.ModLoader.Filesystem;

namespace Aimless.ModLoader.Core.Discovery;

public class FilesystemAssemblyProvider : IAssemblyProvider {

    private readonly IFilesystem _filesystem;

    public FilesystemAssemblyProvider(IFilesystem filesystem) {
        this._filesystem = filesystem;
    }
    
    public IEnumerable<Assembly> GetAssemblies() {
        foreach (var file in _filesystem.ListFiles("", "*.dll")) {
            using var stream = _filesystem.OpenFile(file);
            using var memStream = new MemoryStream();
            stream!.CopyTo(memStream);
            yield return Assembly.Load(memStream.ToArray());
        }
    }
}