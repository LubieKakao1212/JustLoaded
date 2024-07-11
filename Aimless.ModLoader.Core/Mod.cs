using System.Reflection;

namespace Aimless.ModLoader.Core;

public class Mod {

    /// <summary>
    /// Metadata of the mod
    /// </summary>
    public ModMetadata Metadata { get; private set; }

    /// <summary>
    /// Initializer instance used to initialize the mod
    /// </summary>
    public IModInitializer Initializer { get; private set; }

    public IReadOnlyList<Assembly> Assemblies => _assemblies;

    private readonly List<Assembly> _assemblies = new();
    
    public Mod(ModMetadata metadata) {
        this.Metadata = metadata;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initializer"></param>
    public void AddInitializer(IModInitializer initializer) {
        if (this.Initializer != null) {
            //TODO Exception
            throw new Exception("A mod cannot have more than one initializer");
        }
        
        this.Initializer = initializer;
    }

    public void AddAssembly(Assembly assembly) {
        if (_assemblies.Contains(assembly)) {
            Console.Error.WriteLine("Duplicate assembly in " + Metadata.ModKey);
            return;
        }
        
        _assemblies.Add(assembly);
    }
    
}