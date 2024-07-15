namespace JustLoaded.Core.Discovery;

public class ConstantModProvider : IModProvider {

    private readonly Mod[] _mods;
    
    public ConstantModProvider(params Mod[] mods) {
        this._mods = mods;
    }

    public IEnumerable<Mod> DiscoverMods() {
        return _mods;
    }
}