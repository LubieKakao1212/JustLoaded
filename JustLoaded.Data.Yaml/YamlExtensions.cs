using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace JustLoaded.Data.Yaml;

public static class YamlExtensions {

    public static YamlNode Clone(this YamlNode node) => node switch {
        YamlScalarNode scalar => scalar.Clone(),
        YamlSequenceNode sequence => sequence.Clone(),
        YamlMappingNode mapping => mapping.Clone(),
        _ => throw new YamlException($"Cannot clone node {node}, type does not support cloning")
    };

    public static YamlSequenceNode Clone(this YamlSequenceNode sequence) {
        return new YamlSequenceNode(
            sequence.Select(
                node => node.Clone()
                )
            );
    }

    public static YamlMappingNode Clone(this YamlMappingNode mapping) {
        return new YamlMappingNode(
            mapping.Select(
                pair => new KeyValuePair<YamlNode, YamlNode>(
                    pair.Key.Clone(),
                    pair.Value.Clone()
                    )
                )
            );
    }

    public static YamlScalarNode Clone(this YamlScalarNode scalar) {
        return new YamlScalarNode(scalar.Value);
    }
    
}