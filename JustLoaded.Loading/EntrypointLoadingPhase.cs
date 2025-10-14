using JustLoaded.Core;
using JustLoaded.Core.Loading;
using JustLoaded.Core.Reflect;

namespace JustLoaded.Loading;

public abstract class EntrypointLoadingPhase<TEntrypoint> : ILoadingPhase {

    protected EntrypointLoadingPhase() { }
    
    public void Load(ModLoaderSystem modLoader) {
        var mods = modLoader.Mods;
        
        Setup(modLoader);
        foreach (var mod in mods) {
            bool foundEntrypoint = false;
            foreach (var assembly in mod.Assemblies) {
                foreach (var entrypointType in assembly.GetModTypeByBase<TEntrypoint>(ModMetadata.ToModId(mod.Metadata.ModKey))) {
                    if (foundEntrypoint) {
                        throw new ApplicationException($"Duplicate entrypoint {typeof(TEntrypoint)} for mod {mod.Metadata.ModKey}");
                    }
                    foundEntrypoint = true;

                    var entrypoint = mod.GetGlobalObject<TEntrypoint>(entrypointType, () => Activator.CreateInstance(entrypointType)!);
                    
                    HandleEntrypointFor(mod, entrypoint, modLoader);
                }
            }
        }
        Finish(modLoader);
    }
    
    protected virtual void Setup(ModLoaderSystem modLoader) { }

    protected abstract void HandleEntrypointFor(Mod mod, TEntrypoint entrypoint, ModLoaderSystem modLoader);

    protected virtual void Finish(ModLoaderSystem modLoader) { }

}