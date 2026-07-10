namespace JustLoaded.Serialization.Tree;

public static class TreeHelper {

    public static void AssertRoot(this ITreeNode node) {
        if (!node.IsRoot) {
            throw new ApplicationException($"Cannot reparent {nameof(ITreeNode)} instances which are not root (TODO change this exception to a custom one)");
        }
    }

    internal static void AssertNotPresent(int idx, string key) {
        if (idx > -1) {
            throw new ApplicationException($"Cannot add element, key {key} already exists");
        }
    }
    
}