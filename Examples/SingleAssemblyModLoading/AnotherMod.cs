using Aimless.ModLoader.Core;
using Aimless.ModLoader.Core.Reflect;
using Aimless.ModLoader.Util;

namespace SingleAssemblyModLoading;

[ModRelation("non-existing-mod", ModDependencyType.Required, Order.After)]
[ModRelation(ExplosionMod.ModId, ModDependencyType.Required, Order.After)]
[Mod(ModId)]
public class AnotherMod {
    public const string ModId = "another";
}