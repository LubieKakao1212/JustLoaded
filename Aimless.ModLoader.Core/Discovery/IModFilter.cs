namespace Aimless.ModLoader.Core.Discovery;

/// <summary>
/// <see cref="IModFilter"/> implementetions are used to restrict which discovered mods are loaded, this happens BEFORE dependency resolving
/// </summary>
public interface IModFilter {
    
    /// <param name="mods">Unordered set of mods to be filtered, this can be modified to avoid unnecessary allocations</param>
    /// <returns>Can be same instance as <paramref name="mods"/></returns>
    HashSet<Mod> FilterMods(HashSet<Mod> mods);

}