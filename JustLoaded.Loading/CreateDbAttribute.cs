namespace JustLoaded.Loading;

[AttributeUsage(validOn: AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class CreateDbAttribute : Attribute {

    public string ContentId { get; }

    public CreateDbAttribute(string contentId) {
        ContentId = contentId;
    }
    
}