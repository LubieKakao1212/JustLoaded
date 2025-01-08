// See https://aka.ms/new-console-template for more information

using JustLoaded.Core;
using JustLoaded.Core.Discovery;
using JustLoaded.Logger;

Console.WriteLine("Hello, World!");

using var loggerBase = new Logger(
    new ConsoleLogModule()
);

var ml = new ModLoaderSystem.Builder(new AssemblyModProvider(new LoadedAssemblyProvider())).Build()
    .AddAttachment<ILogger>(loggerBase);

var logger = ml.GetRequiredAttachment<ILogger>();

try {
    ml.DiscoverMods();
    ml.ResolveDependencies();
    ml.InitMods();
    ml.Load();
}
catch (Exception e) {
    logger.Error(e.ToString());
}

logger.Info(""+ml.CurrentInitPhase);
