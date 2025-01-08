using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Logger;

namespace JustLoaded.Core.Loading;

public class ListModsLoadingPhase : ILoadingPhase {
    
    public void Load(ModLoaderSystem modLoader) {
        var mods = (ContentDatabase<Mod>)modLoader.MasterDb.GetDatabase<Mod>(new ContentKey("core:mods"));

        var logger = modLoader.GetRequiredAttachment<ILogger>();
        
        logger.Info("Sorted mod list:");
        foreach (var modKey in mods.ContentKeys) {
            logger.Info(modKey.path);
        }
    }
    
}