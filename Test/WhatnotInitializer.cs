using Aimless.ModLoader.Content;
using Aimless.ModLoader.Core;
using Aimless.ModLoader.Core.Loading;
using Aimless.ModLoader.Util;

namespace Test;

public class WhatnotInitializer : IModInitializer {
    
    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        phases.BeginRegister(new ContentKey("whatnot", "whatnot"), new WhatnotPhase())
            .WithOrder(TestModInitializer.Key, Order.After)
            .Register();
    }

    public class WhatnotPhase : ILoadingPhase {
        
        public void Load(ModLoaderSystem modLoader) {
            Console.Error.WriteLine("Whatnot");
        }
        
    }
}