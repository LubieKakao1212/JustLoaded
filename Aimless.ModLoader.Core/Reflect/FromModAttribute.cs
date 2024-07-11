namespace Aimless.ModLoader.Core.Reflect;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class FromModAttribute : Attribute {

    public readonly String modId;
    
    public FromModAttribute(String modId) {
        this.modId = modId;
    }
}