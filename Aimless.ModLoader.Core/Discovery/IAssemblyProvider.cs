using System.Reflection;

namespace Aimless.ModLoader.Core.Discovery;

public interface IAssemblyProvider {

    public IEnumerable<Assembly> GetAssemblies();

}