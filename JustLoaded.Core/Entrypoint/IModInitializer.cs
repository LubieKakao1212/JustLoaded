using JustLoaded.Util.Algorithm;
using JustLoaded.Core.Loading;

namespace JustLoaded.Core;

public interface IModInitializer {

    /// <summary>
    /// Used to register <see cref="Loading.ILoadingPhase"/> to <paramref name="mlSystem"/>
    /// </summary>
    void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases);

}