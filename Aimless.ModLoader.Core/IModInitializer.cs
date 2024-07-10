using Aimless.ModLoader.Core.Loading;
using Aimless.ModLoader.Util.Algorithm;

namespace Aimless.ModLoader.Core;

public interface IModInitializer<TModLoaderSystem> where TModLoaderSystem : IModLoaderSystem {

    /// <summary>
    /// Used to register <see cref="Loading.ILoadingPhase"/> to <paramref name="mlSystem"/>
    /// </summary>
    void SystemInit(TopoSorter<ILoadingPhase<TModLoaderSystem>> phases);

}