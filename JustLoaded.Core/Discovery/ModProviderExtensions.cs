namespace JustLoaded.Core.Discovery;

public static class ModProviderExtensions {
    
    public static IModProvider WithMods(this IModProvider modProvider, params Mod[] mods) {
        return new CombinedModProvider(modProvider, new ConstantModProvider(mods));
    }

    public static IModProvider Verbose(this IModProvider modProvider) {
        return new LoggingModProvider(modProvider);
    }
    
}
