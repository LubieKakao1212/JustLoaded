using JustLoaded.Core;
using JustLoaded.Core.Reflect;
using JustLoaded.Discovery.Reflect;
using JustLoaded.Util;

namespace SingleAssemblyModLoading;

[ModRelation("non-existing-mod", ModDependencyType.Required, Order.After)]
[ModRelation(ExplosionMod.ModId, ModDependencyType.Required, Order.After)]
[Mod(ModId)]
public class AnotherMod {
    public const string ModId = "another";
}