namespace JustLoaded.Core.Discovery;

public class CombinedModProvider : IModProvider {

    private readonly IModProvider[] _modProviders;

    public CombinedModProvider(params IModProvider[] modProviders) {
        this._modProviders = modProviders;
    }
    
    public IEnumerable<Mod> DiscoverMods() {
        return _modProviders.SelectMany((provider) => provider.DiscoverMods());
    }
}