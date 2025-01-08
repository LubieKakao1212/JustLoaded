using JustLoaded.Logger;

namespace JustLoaded.Core.Discovery;

public class LoggingModProvider : IModProvider {
    private readonly IModProvider _innerModProvider;
    private readonly ILogger _logger;

    public LoggingModProvider(IModProvider innerModProvider, ILogger logger) {
        _innerModProvider = innerModProvider;
        _logger = logger;
    }

    public IEnumerable<Mod> DiscoverMods() {
        foreach (var mod in _innerModProvider.DiscoverMods()) {
            _logger.Info($"Found {mod.Metadata.ModKey}");
            yield return mod;
        }
    }
}