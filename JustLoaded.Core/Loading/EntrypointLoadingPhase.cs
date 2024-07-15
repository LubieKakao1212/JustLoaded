using JustLoaded.Core.Reflect;
using JustLoaded.Content;
using JustLoaded.Content.Database;

namespace JustLoaded.Core.Loading;

public abstract class EntrypointLoadingPhase<TEntrypoint> : ILoadingPhase {

    protected EntrypointLoadingPhase() { }
    
    public void Load(ModLoaderSystem modLoader) {
        var mods = (ContentDatabase<Mod>?)modLoader.MasterDb.GetContent(new ContentKey("core:mods"));
        
        if (mods == null) {
            Console.Error.WriteLine("Impossible situation occured.");
            return;
        }
        
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