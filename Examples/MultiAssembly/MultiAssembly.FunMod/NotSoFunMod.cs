using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Core.Reflect;
using MultiAssembly.Game;

namespace MultiAssembly.FunMod;

[FromMod(ModId)]
[ContentContainer]
[Mod(ModId)]
public class NotSoFunMod {
    public const string ModId = "not-fun";

    [RegisterContent] public static Item item = new Item();
    [RegisterContent] public static Item anotherItem = new Item();
    [RegisterContent] public static Item fancy_item = new Item();
    [RegisterContent] public static Item fancy_Item = new Item();

    [RegisterContent] private static string wrong = "dso andwao n";
}

[FromMod(NotSoFunMod.ModId)]
public class RegisterDb : IDatabaseRegisterer {
    
    public void RegisterDatabases(IDatabaseRegistrationContext context) {
        context.CreateDatabase<Item>(new ContentKey(NotSoFunMod.ModId, "item"));
    }
    
}
