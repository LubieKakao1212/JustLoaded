// See https://aka.ms/new-console-template for more information

using JustLoaded.Core;
using JustLoaded.Core.Discovery;

Console.WriteLine("Hello, World!");

var ml = new ModLoaderSystem.Builder(new AssemblyModProvider(new LoadedAssemblyProvider())).Build();

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
