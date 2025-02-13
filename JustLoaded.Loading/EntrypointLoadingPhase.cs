using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Core.Loading;
using JustLoaded.Core.Reflect;

namespace JustLoaded.Loading;

public abstract class EntrypointLoadingPhase<TEntrypoint> : ILoadingPhase {

    protected EntrypointLoadingPhase() { }
    
    public void Load(ModLoaderSystem modLoader) {
        var mods = (ContentDatabase<Mod>)modLoader.MasterDb.GetDatabase<Mod>(new ContentKey("core:mods"));
        
        Setup(modLoader);
        foreach (var modEntry in mods.ContentEntries) {
            var mod = modEntry.Value;
            bool foundEntrypoint = false;
            foreach (var assembly in mod.Assemblies) {
                foreach (var entrypointType in assembly.GetModTypeByBase<TEntrypoint>(ModMetadata.ToModId(modEntry.Key))) {
                    if (foundEntrypoint) {
                        throw new ApplicationException($"Duplicate entrypoint {typeof(TEntrypoint)} for mod {modEntry.Key}");
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