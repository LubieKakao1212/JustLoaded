using JustLoaded.Util.Algorithm;
using JustLoaded.Core.Loading;

namespace JustLoaded.Core;

public class DefaultModInitializer : IModInitializer {

    public static readonly DefaultModInitializer Instance = new DefaultModInitializer();
    
    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        
    }
    
}