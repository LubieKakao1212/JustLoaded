namespace JustLoaded.Util.Attachment;

public class AttachmentProviderImpl : AttachmentProviderBase, IMutableAttachmentProvider<AttachmentProviderImpl> {

    AttachmentProviderImpl IMutableAttachmentProvider<AttachmentProviderImpl>.AddAttachment<T>(T attachment) {
        base.AddAttachment(attachment);
        return this;
    }

    T IMutableAttachmentProvider<AttachmentProviderImpl>.GetOrAddAttachment<T>(Func<T> constructor) {
        return base.GetOrAddAttachment(constructor);
    }
}