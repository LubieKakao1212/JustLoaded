using Aimless.ModLoader.Content.Database;
using Aimless.ModLoader.Core.Loading;

namespace Aimless.ModLoader.Core;

public interface IModLoaderSystem {
    //Empty
}

public class ModLoaderSystem<TModMeta, TModInit> : IModLoaderSystem where TModMeta : ModMetadata where TModInit : IModInitializer<ModLoaderSystem<TModMeta, TModInit>> {

    private MasterDatabase _masterDB;

    private readonly ArrayDatabase<ILoadingPhase<ModLoaderSystem<TModMeta, TModInit>>> _loadingPhases = new();

    private readonly ArrayDatabase<Mod<TModMeta, TModInit, ModLoaderSystem<TModMeta, TModInit>>> _mods = new();
    
    public ModLoaderSystem(MasterDatabase masterDB) {
        this._masterDB = masterDB;
    }
    
    public void DiscoverMods() {
        
    }

}