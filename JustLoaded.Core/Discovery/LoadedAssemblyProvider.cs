using System.Reflection;

namespace JustLoaded.Core.Discovery;

public class LoadedAssemblyProvider : IAssemblyProvider {
    
    public IEnumerable<Assembly> GetAssemblies() {
        return AppDomain.CurrentDomain.GetAssemblies();
    }
    
}