using System.Reflection;
using JustLoaded.Core.Entrypoint;

namespace JustLoaded.Core;

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

    private readonly Dictionary<Type, object> _globalObjects = new();
    
    public Mod(ModMetadata metadata) {
        this.Metadata = metadata;
        this.Initializer = DefaultModInitializer.Instance;
    }
    
    public Mod AddInitializer(IModInitializer initializer) {
        if(this.Initializer != DefaultModInitializer.Instance) {
            //TODO Exception
            throw new Exception("A mod cannot have more than one initializer");
        }
        
        this.Initializer = initializer;
        return this;
    }

    public Mod AddAssembly(Assembly assembly) {
        if (_assemblies.Contains(assembly)) {
            Console.Error.WriteLine("Duplicate assembly in " + Metadata.ModKey);
            return this;
        }
        
        _assemblies.Add(assembly);
        return this;
    }

    public TCast GetGlobalObject<TCast>(Type objType, Func<object> constructor) {
        if (_globalObjects.TryGetValue(objType, out var obj)) {
            return (TCast)obj;
        }
        var newObj = constructor();
        if (newObj.GetType() != objType) {
            //TODO exception
            throw new ApplicationException("Type Mismatch");
        }
        _globalObjects.Add(objType, newObj);
        return (TCast)newObj;
    }
}