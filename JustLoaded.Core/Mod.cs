using System.Reflection;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Util.Attachment;

namespace JustLoaded.Core;

public class Mod(ModMetadata metadata) : IMutableAttachmentProvider<Mod> {

    /// <summary>
    /// Metadata of the mod
    /// </summary>
    public ModMetadata Metadata { get; private set; } = metadata;

    /// <summary>
    /// Initializer instance used to initialize the mod
    /// </summary>
    public IModInitializer Initializer { get; private set; } = DefaultModInitializer.Instance;

    public IReadOnlyList<Assembly> Assemblies => _assemblies;

    private readonly List<Assembly> _assemblies = new();

    private readonly Dictionary<Type, object> _globalObjects = new();

    private readonly IMutableAttachmentProvider<AttachmentProviderImpl> _attachmentProviderImpl = new AttachmentProviderImpl();

    public Mod AddInitializer(IModInitializer initializer) {
        if(this.Initializer != DefaultModInitializer.Instance) {
            //TODO Exception
            throw new Exception("A mod cannot have more than one initializer");
        }
        
        this.Initializer = GetGlobalObject<IModInitializer>(initializer.GetType(), () => initializer);
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

    /// <summary>
    /// Used for deduplication of reflection loaded objects
    /// </summary>
    /// <param name="objType">The real type of object to be created</param>
    /// <param name="constructor">Lazy constructor for <paramref name="objType"/>, must return an object of type equal to <paramref name="objType"/> </param>
    /// <typeparam name="TCast">Type to which to cast the result, <paramref name="objType"/> must inherit or implement <typeparamref name="TCast"/></typeparam>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown when value returned by <paramref name="constructor"/> is of a different type than <paramref name="objType"/></exception>
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

    public T? GetAttachment<T>() where T : class {
        return _attachmentProviderImpl.GetAttachment<T>();
    }

    public T GetRequiredAttachment<T>() where T : class {
        return _attachmentProviderImpl.GetRequiredAttachment<T>();
    }

    public bool HasAttachment<T>() where T : class {
        return _attachmentProviderImpl.HasAttachment<T>();
    }

    public Mod AddAttachment<T>(T attachment) where T : class {
        _attachmentProviderImpl.AddAttachment(attachment);
        return this;
    }

    public T GetOrAddAttachment<T>(Func<T> constructor) where T : class {
        return _attachmentProviderImpl.GetOrAddAttachment(constructor);
    }
}