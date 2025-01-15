using System.Reflection;

namespace JustLoaded.Discovery.Reflect;

public interface IAssemblyProvider {

    public IEnumerable<Assembly> GetAssemblies();

}