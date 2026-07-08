namespace JustLoaded.Util.Attachment;

public interface IRecursiveAttachmentProvider : IAttachmentProvider {
    
    T? GetAttachmentRecursive<T>() where T : class;

    T GetRequiredAttachmentRecursive<T>() where T : class;

    bool HasAttachmentRecursive<T>() where T : class;
    
}