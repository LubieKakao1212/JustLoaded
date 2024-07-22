using JustLoaded.Content;
using JustLoaded.Content.Database;

namespace JustLoaded.Core.Loading;

public class ListModsLoadingPhase : ILoadingPhase {
    
    public void Load(ModLoaderSystem modLoader) {
        var mods = (ContentDatabase<Mod>)modLoader.MasterDb.GetDatabase<Mod>(new ContentKey("core:mods"));
             
        foreach (var modKey in mods.ContentKeys) {
            Console.WriteLine(modKey);
        }
    }
    
}