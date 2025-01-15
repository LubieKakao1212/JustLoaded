using System.Reflection;

namespace JustLoaded.Discovery.Reflect;

public class LoadedAssemblyProvider : IAssemblyProvider {
    
    public IEnumerable<Assembly> GetAssemblies() {
        return AppDomain.CurrentDomain.GetAssemblies();
    }
    
}