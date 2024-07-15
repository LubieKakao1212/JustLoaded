namespace JustLoaded.Core.Discovery;

public class LoggingModProvider : IModProvider {

    private readonly IModProvider _innerModProvider;

    public LoggingModProvider(IModProvider innerModProvider) {
        this._innerModProvider = innerModProvider;
    }
    
    public IEnumerable<Mod> DiscoverMods() {
        foreach (var mod in _innerModProvider.DiscoverMods()) {
            //TODO Logger
            Console.Out.WriteLine($"Found {mod.Metadata.ModKey}");
            yield return mod;
        }
    }
}