using YamlDotNet.Core.Events;

namespace JustLoaded.Serialization.Tree;

public interface ITreeNode {

    ITreeNode? Parent { get; }
    
    bool IsRoot => Parent == null;

    /// <summary>
    /// Can be set to an arbitrary value, used as custom metadata.
    /// </summary>
    //TODO think about something more robust 
    object? Companion { get; set; }

    /// <summary>
    /// Returns <see cref="Companion"/> or if it was null attempts to acquire it from <see cref="Parent"/>
    /// </summary>
    /// <returns></returns>
    object? GetCompanionRecursive() => Companion ?? Parent?.GetCompanionRecursive();

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
}