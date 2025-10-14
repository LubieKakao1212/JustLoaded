using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Core.Loading;
using JustLoaded.Logger;

namespace JustLoaded.Loading;

public class ListModsLoadingPhase : ILoadingPhase {
    
    public void Load(ModLoaderSystem modLoader) {
        var mods = modLoader.Mods;
        var logger = modLoader.GetRequiredAttachment<ILogger>();
        
        logger.Info("Sorted mod list:");
        foreach (var modKey in mods) {
            logger.Info(modKey.Metadata.ModKey.path);
        }
    }
    
}