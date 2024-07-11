namespace Aimless.ModLoader.Core.Discovery;

/// <summary>
/// <see cref="IModProvider"/> implementation are used to "discover" mods which are available for the game to load 
/// </summary>
public interface IModProvider {

    /// <returns>Mods discovered by this provider in no particular order</returns>
    IEnumerable<Mod> DiscoverMods();

}