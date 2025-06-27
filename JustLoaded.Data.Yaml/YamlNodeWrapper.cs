using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace JustLoaded.Data.Yaml;

public class YamlNodeWrapper {

    public readonly YamlNode RawNode;
    public readonly YamlDocument RawRoot;

    public readonly YamlNodeWrapper Parent;
    public readonly YPath CurrentPath;
    
    // private YamlNodeWrapper[] 
    
    private YamlNodeWrapper(YamlNode rawNode, YamlDocument rawRoot, YamlNodeWrapper parent, YPath currentPath) {
        RawNode = rawNode;
        RawRoot = rawRoot;
        Parent = parent;
        CurrentPath = currentPath;
    }
    
    /// <summary>
    /// Returns an array with children of this node or null if node is a scalar
    /// </summary>
    /// <returns></returns>
    public YamlNodeWrapper[]? GetChildren() {
        return GetValue(
            scalar => null,
            sequence => {
                int i = 0;
                return sequence.Children.Select(node => GetAsIndexedChild(node, i++)).ToArray();
            },
            mapping => mapping.Children.Select(node => GetAsKeyedChild(node.Value, ((YamlScalarNode)node.Key).Value!)).ToArray());
    }
    
    public T? GetValue<T>(Func<YamlScalarNode, T?> scalarHandler, Func<YamlSequenceNode, T?> sequenceHandler, Func<YamlMappingNode, T?> mappingHandler) {
        switch (RawNode.NodeType) {
            case YamlNodeType.Scalar:
                return scalarHandler((YamlScalarNode) RawNode);
            case YamlNodeType.Sequence:
                return sequenceHandler((YamlSequenceNode) RawNode);
            case YamlNodeType.Mapping:
                return mappingHandler((YamlMappingNode) RawNode);
            default:
                throw new YamlException(RawNode.Start, RawNode.End, "Unsupported node type");
        }
    }
    

    private YamlNodeWrapper GetAsIndexedChild(YamlNode node, int index) {
        return new YamlNodeWrapper(
            node,
            RawRoot,
            this,
            CurrentPath.WithIndex(index)
            );
    }

    private YamlNodeWrapper GetAsKeyedChild(YamlNode node, string key) {
        return new YamlNodeWrapper(
            node,
            RawRoot,
            this,
            CurrentPath.WithKey(key)
            );
    }
}