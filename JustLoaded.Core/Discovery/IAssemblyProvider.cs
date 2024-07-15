using System.Reflection;

namespace JustLoaded.Core.Discovery;

public interface IAssemblyProvider {

    public IEnumerable<Assembly> GetAssemblies();

}