using JustLoaded.Logger;

namespace JustLoaded.Core.Discovery;

public static class ModProviderExtensions {
    
    public static IModProvider WithMods(this IModProvider modProvider, params Mod[] mods) {
        return new CombinedModProvider(modProvider, new ConstantModProvider(mods));
    }

    public static IModProvider Verbose(this IModProvider modProvider, ILogger logger) {
        return new LoggingModProvider(modProvider, logger);
    }
    
}
