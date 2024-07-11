using Aimless.ModLoader.Content;
using Aimless.ModLoader.Content.Database;
using Aimless.ModLoader.Core;
using Aimless.ModLoader.Core.Loading;

namespace Test;

public class PrintModsLoadingPhase : ILoadingPhase {
    
    public void Load(ModLoaderSystem modLoader) {
        var mods = (ContentDatabase<Mod>?)modLoader.MasterDb.GetContent(new ContentKey("core:mods"));

        if (mods == null) {
            Console.Error.WriteLine("Impossible situation occured.");
            return;
        }
        
        foreach (var modKey in mods.ContentKeys) {
            Console.WriteLine(modKey.path);
        }
    }
    
}