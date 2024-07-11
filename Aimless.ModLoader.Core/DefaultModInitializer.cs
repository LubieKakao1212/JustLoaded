using Aimless.ModLoader.Core.Loading;
using Aimless.ModLoader.Util.Algorithm;

namespace Aimless.ModLoader.Core;

public class DefaultModInitializer : IModInitializer {

    public static readonly DefaultModInitializer Instance = new DefaultModInitializer();
    
    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        
    }
    
}