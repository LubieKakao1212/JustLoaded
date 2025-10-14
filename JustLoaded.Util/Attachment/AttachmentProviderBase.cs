using JustLoaded.Util.Extensions;

namespace JustLoaded.Util.Attachment;

public class AttachmentProviderBase : IAttachmentProvider {
    private readonly Dictionary<Type, object> _attachments = new();
    
    protected void AddAttachment<T>(T attachment) where T : class {
        _attachments.Add(typeof(T), attachment);
    }

    protected T GetOrAddAttachment<T>(Func<T> constructor) where T : class {
        return (T)_attachments.GetValueOrSetDefaultLazy(typeof(T), constructor);
    }
    
    public T? GetAttachment<T>() where T : class {
        return _attachments.GetValueOrDefault(typeof(T)) as T;
    }

    public T GetRequiredAttachment<T>() where T : class {
        return GetAttachment<T>() ?? throw new ApplicationException($"No attachment of type {typeof(T)}");
    }

    public bool HasAttachment<T>() where T : class {
        return _attachments.ContainsKey(typeof(T));
    }

    protected void CopyFrom(AttachmentProviderBase otherProvider) {
        foreach (var attachment in otherProvider._attachments) {
            _attachments.Add(attachment.Key, attachment.Value);
        }
    }
}