using JustLoaded.Core;
using JustLoaded.Core.Discovery;

namespace Test;

public class TestModProvider : IModProvider {

    private readonly Mod[] _mods;
    
    public TestModProvider(Mod[] mods) {
        this._mods = mods;
    }
    
    public IEnumerable<Mod> DiscoverMods() {
        return _mods;
    }
    
}