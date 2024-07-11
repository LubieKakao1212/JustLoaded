using Aimless.ModLoader.Core;
using Aimless.ModLoader.Core.Discovery;

namespace Test;

public class ExampleModFilter : IModFilter {

    private HashSet<string> _badIds;
    
    public ExampleModFilter(params string[] badIds) {
        this._badIds = new HashSet<string>(badIds);
    }
    
    public HashSet<Mod> FilterMods(HashSet<Mod> mods) {
        mods.RemoveWhere((mod) => _badIds.Contains(mod.Metadata.ModKey.path));
        return mods;
    }
    
}