using Aimless.ModLoader.Content;
using Aimless.ModLoader.Core;
using Aimless.ModLoader.Core.Loading;
using Aimless.ModLoader.Util;

namespace Test;

public class TestModInitializer : IModInitializer {
    
    public static readonly ContentKey Key = new ContentKey("coremod", "print");
    
    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        phases.New(Key, new PrintModsLoadingPhase())
            .Register();
    }
    
}