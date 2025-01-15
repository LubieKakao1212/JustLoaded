using System.Reflection;
using JustLoaded.Core;
using JustLoaded.Core.Discovery;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Reflect;
using JustLoaded.Util;

namespace JustLoaded.Discovery.Reflect;

public class AssemblyModProvider : IModProvider {

    private readonly IAssemblyProvider _assemblyProvider;
    
    public AssemblyModProvider(IAssemblyProvider assemblyProvider) {
        this._assemblyProvider = assemblyProvider;
    }
    
    public IEnumerable<Mod> DiscoverMods() {
        var assemblies = _assemblyProvider.GetAssemblies();

        var mods = new List<Mod>();
        
        foreach (var assembly in assemblies) {
            var modTypes = assembly.GetTypesByAttribute<ModAttribute>();
            foreach (var modType in modTypes) {
                var id = modType.GetCustomAttribute<ModAttribute>()!.modId;
                var deps = modType.GetCustomAttributes<ModRelationAttribute>();
                var metaBuilder = new ModMetadata.Builder(id);
                var depSet = new HashSet<string>();
                
                foreach (var dep in deps) {
                    if (!depSet.Add(dep.relatedModId)) {
                        //TODO use logger (Warning)
                        Console.Error.WriteLine($"Duplicate dependency for { id } on { dep }");
                        continue;
                    }
                        
                    if (dep.type == ModDependencyType.Required) {
                        metaBuilder.AddRequiredDependency(dep.order, dep.relatedModId);
                    }
                    else {
                        metaBuilder.AddOptionalDependency(dep.order, dep.relatedModId);
                    }
                }

                IModInitializer? init = null;

                foreach (var initializerType in assembly.GetModTypeByBase<IModInitializer>(id)) {
                    if (init != null) {
                        Console.Error.WriteLine($"Duplicate IModInitializer found for { id }");
                    }

                    init = (IModInitializer?)Activator.CreateInstance(initializerType);
                }
                
                var mod = new Mod(metaBuilder.Build());
                mod.AddAssembly(assembly);
                if (init != null) {
                    mod.AddInitializer(init);
                }
                mods.Add(mod);
            }
        }

        return mods;
    }
}