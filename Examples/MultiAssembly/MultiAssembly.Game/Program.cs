using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Core.Discovery;
using JustLoaded.Filesystem;
using MultiAssembly.Game;

Console.WriteLine("Hello, World!");

var fs = new PhysicalFilesystem("mods".AsPath());

var ml = new ModLoaderSystem.Builder(
    new AssemblyModProvider(
        new FilesystemAssemblyProvider(fs)
        )
    ).Build();

try {
    ml.DiscoverMods();
    ml.ResolveDependencies();
    ml.InitMods();
    ml.Load();
}
catch (Exception e) {
    Console.WriteLine(e);
}

Console.WriteLine(ml.CurrentInitPhase);

var items = (IContentDatabase<Item>?)ml.MasterDb.GetByContentType<Item>();

foreach (var key in items!.ContentKeys) {
    Console.WriteLine("Item: "+key);
}



