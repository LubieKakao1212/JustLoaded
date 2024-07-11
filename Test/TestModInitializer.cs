using Aimless.ModLoader.Content;
using Aimless.ModLoader.Core;
using Aimless.ModLoader.Core.Loading;

namespace Test;

public class TestModInitializer : IModInitializer {
    
    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        phases.BeginRegister(
            new ContentKey(thisMod.Metadata.ModKey.path, "print"), new PrintModsLoadingPhase()
            ).Register();
    }
    
}