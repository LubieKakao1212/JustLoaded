namespace Aimless.ModLoader.Core.Loading;

public interface ILoadingPhase<in TModLoaderSystem> where TModLoaderSystem : IModLoaderSystem {

    public void Load(TModLoaderSystem modLoader);

}