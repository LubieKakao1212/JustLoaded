namespace JustLoaded.Core.Discovery;

/// <summary>
/// <see cref="IModFilter"/> passing every discovered mod
/// </summary>
public class IdentityModFilter : IModFilter {

    public static readonly IdentityModFilter Instance = new IdentityModFilter();
    
    public HashSet<Mod> FilterMods(HashSet<Mod> mods) {
        return mods;
    }
    
}