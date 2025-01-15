using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Loading;
using JustLoaded.Loading;
using JustLoaded.Util;

namespace Test;

public class TestModInitializer : IModInitializer {
    
    public static readonly ContentKey Key = new ContentKey("coremod", "print");
    
    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        phases.New(Key, new ListModsLoadingPhase())
            .Register();
    }
    
}