// See https://aka.ms/new-console-template for more information

using Aimless.ModLoader.Core;
using Aimless.ModLoader.Core.Discovery;

Console.WriteLine("Hello, World!");

var ml = new ModLoaderSystem.Builder(new AssemblyModProvider(new LoadedAssemblyProvider())).Build();

ml.DiscoverMods();
ml.ResolveDependencies();
ml.InitMods();
ml.Load();

Console.WriteLine(ml.CurrentInitPhase);
