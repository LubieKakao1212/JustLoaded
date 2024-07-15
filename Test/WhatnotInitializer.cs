using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Core.Loading;
using JustLoaded.Util;

namespace Test;

public class WhatnotInitializer : IModInitializer {
    
    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        phases.New(new ContentKey("whatnot", "whatnot"), new WhatnotPhase())
            .WithOrder(TestModInitializer.Key, Order.After)
            .Register();
    }

    public class WhatnotPhase : ILoadingPhase {
        
        public void Load(ModLoaderSystem modLoader) {
            Console.Error.WriteLine("Whatnot");
        }
        
    }
}