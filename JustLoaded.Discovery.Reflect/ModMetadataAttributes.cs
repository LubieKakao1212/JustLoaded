using JustLoaded.Core;
using JustLoaded.Util;

namespace JustLoaded.Discovery.Reflect;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ModAttribute : Attribute {
    
    public readonly String modId;
    
    public ModAttribute(String modId) {
        this.modId = modId;
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class ModRelationAttribute : Attribute {

    public readonly string relatedModId;
    public readonly ModDependencyType type;
    public readonly Order order;
    
    public ModRelationAttribute(string relatedModId, ModDependencyType type, Order order) {
        this.relatedModId = relatedModId;
        this.type = type;
        this.order = order;
    }
}