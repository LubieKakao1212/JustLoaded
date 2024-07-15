using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Core.Loading;

namespace Test;

public class PrintModsLoadingPhase : ILoadingPhase {

    public void Load(ModLoaderSystem modLoader) {
        var mods = (ContentDatabase<Mod>?)modLoader.MasterDb.GetContent(new ContentKey("core:mods"));

        if (mods == null) {
            Console.Error.WriteLine("Impossible situation occured.");
            return;
        }
        
        foreach (var modKey in mods.ContentKeys) {
            Console.WriteLine(modKey);
        }
    }
}