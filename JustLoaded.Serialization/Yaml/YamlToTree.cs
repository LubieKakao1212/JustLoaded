using JustLoaded.Serialization.Tree;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace JustLoaded.Serialization.Yaml;

public static class YamlToTree {
    
    public static DeserializerBuilder WithTreeDeserialization(this DeserializerBuilder builder) {
        return builder
            .WithNodeDeserializer(new TreeDeserializer())
            .WithNodeTypeResolver(new TreeTypeResolver());
    }
    
    public class TreeDeserializer : INodeDeserializer {

        public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value,
            ObjectDeserializer rootDeserializer) {

            value = DeserializeScalar(reader, expectedType, nestedObjectDeserializer) ??
                    DeserializeCollection(reader, expectedType, nestedObjectDeserializer) ??
                    DeserializeMapping(reader, expectedType, nestedObjectDeserializer);
            return value != null;
        }
        
        private static object? DeserializeScalar(IParser reader, Type requestedType, Func<IParser, Type, object?> nestedObjectDeserializer) {
            return If<ITreeScalar>(requestedType, () => {
                var scalar = reader.Consume<Scalar>();
                return new TreeScalar(scalar.Value);
            });
        }

        private static object? DeserializeCollection(IParser reader, Type requestedType, Func<IParser, Type, object?> nestedObjectDeserializer) {
            return If<ITreeCollection>(requestedType, () => {
                var seq = reader.Consume<SequenceStart>();

                var result = new TreeCollection();
                int i = 0;
                while (!reader.TryConsume<SequenceEnd>(out var end)) {
                    result.InsertChild(i++, (ITreeNode)nestedObjectDeserializer(reader, typeof(ITreeNode))!);
                }
                return result;
            });
        }
        
        private static object? DeserializeMapping(IParser reader, Type requestedType, Func<IParser, Type, object?> nestedObjectDeserializer) {
            return If<ITreeMapping>(requestedType, () => {
                var map = reader.Consume<MappingStart>();

                var result = new TreeMapping();
                while (!reader.TryConsume<MappingEnd>(out var end)) {
                    var key = reader.Consume<Scalar>();
                    result.InsertChild(key.Value, (ITreeNode)nestedObjectDeserializer(reader, typeof(ITreeNode))!);
                }
                return result;
            });
        }
        
        private static object? If<TRequest>(Type type, Func<object> action) {
            if (type == typeof(TRequest)) {
                return action();
            }
            return null;
        }

        // public bool TryDiscriminate(IParser buffer, out Type? suggestedType) {
        //     if (buffer.Current is Scalar) {
        //         suggestedType = typeof(ITreeScalar);
        //         return true;
        //     }
        //     if (buffer.Current is SequenceStart) {
        //         suggestedType = typeof(ITreeCollection);
        //         return true;
        //     }
        //     if (buffer.Current is MappingStart) {
        //         suggestedType = typeof(ITreeMapping);
        //         return true;
        //     }
        //     suggestedType = null;
        //     return false;
        // }

        // public Type BaseType { get; } = typeof(ITreeNode);

        
    }

    public class TreeTypeResolver : INodeTypeResolver {
       
        public bool Resolve(NodeEvent? nodeEvent, ref Type currentType) {
            if (nodeEvent is Scalar) {
                currentType = typeof(ITreeScalar);
                return true;
            }
            if (nodeEvent is SequenceStart) {
                currentType = typeof(ITreeCollection);
                return true;
            }
            if (nodeEvent is MappingStart) {
                currentType = typeof(ITreeMapping);
                return true;
            }
            return false;
        }
    }
}