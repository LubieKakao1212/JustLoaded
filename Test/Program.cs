// See https://aka.ms/new-console-template for more information

using Aimless.ModLoader.Core;
using Aimless.ModLoader.Util;
using Test;

Console.WriteLine("Hello, World!");

var modLoader = new ModLoaderSystem.Builder(new TestModProvider(new Mod[] {
    new Mod(ModMetadata.Create("funmod").Build()),
    new Mod(ModMetadata.Create("whatnot").Build()).AddInitializer(new WhatnotInitializer()),
    new Mod(ModMetadata.Create("explosionsmod").AddRequiredDependencies(Order.After, "funmod").Build()),
    new Mod(ModMetadata.Create("coremod").Build()).AddInitializer(new TestModInitializer())
})).Build();

modLoader.DiscoverMods();
modLoader.ResolveDependencies();
modLoader.InitMods();
modLoader.Load();

Console.WriteLine(modLoader.CurrentInitPhase);
