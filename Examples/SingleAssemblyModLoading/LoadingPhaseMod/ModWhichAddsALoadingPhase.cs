using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Loading;
using JustLoaded.Core.Reflect;
using JustLoaded.Discovery.Reflect;
using JustLoaded.Loading;

namespace SingleAssemblyModLoading.LoadingPhaseMod;

[Mod(ModId)]
public class ModWhichAddsALoadingPhase {
    public const string ModId = "loading-phase-mod";
}

[FromMod(ModWhichAddsALoadingPhase.ModId)]
public class ModWhichAddsALoadingPhaseInitializer : IModInitializer {
    
    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        phases.New(new ContentKey(ModWhichAddsALoadingPhase.ModId, "print-mods"), new ListModsLoadingPhase())
            .Register();
    }
    
}