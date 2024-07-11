using System.Reflection;

namespace Aimless.ModLoader.Core.Discovery;

public class LoadedAssemblyProvider : IAssemblyProvider {
    
    public IEnumerable<Assembly> GetAssemblies() {
        return AppDomain.CurrentDomain.GetAssemblies();
    }
    
}