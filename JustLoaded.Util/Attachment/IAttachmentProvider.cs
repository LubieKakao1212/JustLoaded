namespace JustLoaded.Util.Attachment;

public interface IAttachmentProvider {

    T? GetAttachment<T>() where T : class;

    T GetRequiredAttachment<T>() where T : class;

    bool HasAttachment<T>() where T : class;
}