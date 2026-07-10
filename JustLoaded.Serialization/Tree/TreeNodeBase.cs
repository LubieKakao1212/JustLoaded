using JustLoaded.Util.Attachment;
using YamlDotNet.Core.Events;

namespace JustLoaded.Serialization.Tree;

public abstract class TreeNodeBase : ITreeNode {

    public ITreeNode? Parent { get; private set; }

    public object? Companion { get; set; }

    private readonly AttachmentProviderImpl _attachmentProviderImpl = new();
    
    public abstract IEnumerable<ParsingEvent> ToEvents();

    public void SetParentInternal(ITreeNode? newParent) {
        Parent = newParent;
    }

    public abstract ITreeNode DeepClone();

    #region Attachments
    
    public T? GetAttachment<T>() where T : class {
        return _attachmentProviderImpl.GetAttachment<T>();
    }

    public T GetRequiredAttachment<T>() where T : class {
        return _attachmentProviderImpl.GetRequiredAttachment<T>();
    }

    public bool HasAttachment<T>() where T : class {
        return _attachmentProviderImpl.HasAttachment<T>();
    }

    public T? GetAttachmentRecursive<T>() where T : class {
        return GetAttachment<T>() ?? Parent?.GetAttachmentRecursive<T>();
    }

    public T GetRequiredAttachmentRecursive<T>() where T : class {
        return GetAttachment<T>() ?? Parent?.GetAttachmentRecursive<T>() ?? throw new MissingAttachmentException(typeof(T));
    }

    public bool HasAttachmentRecursive<T>() where T : class {
        return HasAttachment<T>() || (Parent?.HasAttachment<T>() ?? false);
    }

    public ITreeNode AddAttachment<T>(T attachment) where T : class {
        ((IMutableAttachmentProvider<AttachmentProviderImpl>)_attachmentProviderImpl).AddAttachment(attachment);
        return this;
    }

    public T GetOrAddAttachment<T>(Func<T> constructor) where T : class {
        return ((IMutableAttachmentProvider<AttachmentProviderImpl>)_attachmentProviderImpl).GetOrAddAttachment(constructor);
    }

    #endregion
    
}