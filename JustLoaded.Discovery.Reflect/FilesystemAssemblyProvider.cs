using System.Reflection;
using JustLoaded.Filesystem;

namespace JustLoaded.Discovery.Reflect;

public class FilesystemAssemblyProvider(IFilesystem filesystem) : IAssemblyProvider
{

    public IEnumerable<Assembly> GetAssemblies()
    {
        foreach (var file in filesystem.ListFiles(".".AsPath(), "*.dll", true))
        {
            using var stream = filesystem.OpenFile(file);
            using var memStream = new MemoryStream();
            stream!.CopyTo(memStream);
            yield return Assembly.Load(memStream.ToArray());
        }
    }
}
