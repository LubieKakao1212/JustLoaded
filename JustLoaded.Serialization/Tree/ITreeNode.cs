using JustLoaded.Util.Attachment;
using YamlDotNet.Core.Events;

namespace JustLoaded.Serialization.Tree;

public interface ITreeNode : IRecursiveAttachmentProvider, IMutableAttachmentProvider<ITreeNode> {

    ITreeNode? Parent { get; }
    
    bool IsRoot => Parent == null;

    /// <summary>
    /// Temporary until dedicated serializer is made
    /// </summary>
    /// <returns></returns>
    IEnumerable<ParsingEvent> ToEvents();

    /// <summary>
    /// Does not check remove itself from previous parent, does not add itself to a new parent
    /// </summary>
    /// <param name="newParent"></param>
    internal void SetParentInternal(ITreeNode? newParent);

    /// <summary>
    /// Clones this <see cref="ITreeNode"/> and all of its children <br/>
    /// Does not clone attachments
    /// </summary>
    /// <returns>Cloned node as root</returns>
    ITreeNode DeepClone();
}