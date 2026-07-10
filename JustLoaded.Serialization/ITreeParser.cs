using JustLoaded.Serialization.Tree;

namespace JustLoaded.Serialization;

public interface ITreeParser {

    public ITreeNode ParseTree(Stream stream);

}