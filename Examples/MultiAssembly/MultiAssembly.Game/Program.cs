using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Discovery.Reflect;
using JustLoaded.Filesystem;
using JustLoaded.Logger;
using MultiAssembly.Game;

Console.WriteLine("Hello, World!");

var fs = new RelativeFilesystem(new PhysicalFilesystem(PathExtensions.Local), "mods".AsPath());

using var loggerBase = new Logger(
    new ConsoleLogModule()
);

var ml = new ModLoaderSystem.Builder(
    new AssemblyModProvider(
        new FilesystemAssemblyProvider(fs)
        )
    ).Build().AddAttachment<ILogger>(loggerBase);

try {
    ml.DiscoverMods();
    ml.ResolveDependencies();
    ml.InitMods();
    ml.Load();
}
catch (Exception e) {
    Console.WriteLine(e);
}

var log = ml.GetRequiredAttachment<ILogger>();

log.Info(""+ml.CurrentInitPhase);

var items = (IContentDatabase<Item>?)ml.MasterDb.GetByContentType<Item>();


foreach (var key in items!.ContentKeys) {
    log.Info("Item: "+key);
}



