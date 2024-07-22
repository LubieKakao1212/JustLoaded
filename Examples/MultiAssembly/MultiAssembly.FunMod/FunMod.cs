using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Loading;
using JustLoaded.Core.Reflect;
using JustLoaded.Util;

namespace MultiAssembly.FunMod;

[Mod(ModId)]
public class FunMod {
    public const string ModId = "fun-mod";
}

[FromMod(FunMod.ModId)]
public class FunModInitializer : IModInitializer {
    
    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        var printMods = new ContentKey(FunMod.ModId, "print-mods");
        var registerDb = new ContentKey(FunMod.ModId, "register-db");
        var gatherContent = new ContentKey(FunMod.ModId, "gather-content");
        phases.New(printMods, new ListModsLoadingPhase())
            .Register();
        phases.New(registerDb, new DefaultDatabaseRegistrationEntrypointLoadingPhase())
            .WithOrder(printMods, Order.After)
            .Register();
        phases.New(gatherContent, new RegisterContentLoadingPhase())
            .WithOrder(registerDb, Order.After)
            .WithOrder(new ContentKey("abc", "def"), Order.Before)
            .Register();
    }
}