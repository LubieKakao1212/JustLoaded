namespace JustLoaded.Loading;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class RegisterContentAttribute : Attribute {

    public readonly string? id;

    public readonly string? databaseId;
    
    public RegisterContentAttribute(string? id = null, string? databaseId = null) {
        this.id = id;
        this.databaseId = databaseId;
    }
    
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ContentContainerAttribute : Attribute {
}