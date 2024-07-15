using Aimless.ModLoader.Content;
using Aimless.ModLoader.Core;
using Aimless.ModLoader.Core.Debug;
using Aimless.ModLoader.Core.Loading;
using Aimless.ModLoader.Core.Reflect;

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