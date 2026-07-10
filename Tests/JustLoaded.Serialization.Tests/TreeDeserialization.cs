using System.Diagnostics.CodeAnalysis;
using JustLoaded.Serialization.Tree;
using YamlDotNet.Serialization;

namespace JustLoaded.Serialization.Tests;

public class TreeDeserialization {

    private readonly IDeserializer _deserializer = new DeserializerBuilder()
        .WithEnforceRequiredMembers()
        .Build();
    
    [Fact]
    public void Scalar2Int() {
        var number = 125;
        
        var scalar = new TreeScalar(number.ToString());
        var parser = EnumeratorParser.FromTree(scalar);

        var value = _deserializer.Deserialize<int>(parser);
        
        Assert.Equal(number, value);
    }
    
    [Fact]
    public void CollectionToIntArray() {
        var numbers = new int[] { 125, -144, 2137, 1337 };
        
        var collection = new TreeCollection(numbers.Length);

        for (int i = 0; i < numbers.Length; i++) {
            collection.InsertChild(i, new TreeScalar(numbers[i].ToString()));
        }
        
        var parser = EnumeratorParser.FromTree(collection);

        var result = _deserializer.Deserialize<int[]>(parser);
        Assert.Equal(numbers, result);
    }
    
    [Fact]
    public void CollectionToIntDictionary() {
        var numbers = new int[4] { 125, -144, 2137, 1337 };
        var keys = new String[4] { "ala", "ma", "kota", "2137" };
        
        var mapping  = new TreeMapping();

        for (int i = 0; i < numbers.Length; i++) {
            mapping.InsertChild(keys[i], new TreeScalar(numbers[i].ToString()));
        }
        
        var parser = EnumeratorParser.FromTree(mapping);

        var result = _deserializer.Deserialize<Dictionary<string, int>>(parser);
        
        Assert.Equal(numbers.Length, result.Count);
        for (int i = 0; i < numbers.Length; i++) {
            Assert.Equal(result[keys[i]], numbers[i]);
        }
    }

    [Fact]
    public void MappingToStruct() {
        var expected = new ValueStruct {
            amount = 5,
            name = "Fancy",
            subCounts = [2, 1, 3, 7]
        };

        var root = new TreeMapping();
        root.InsertChild("amount", new TreeScalar(expected.amount.ToString()));
        root.InsertChild("name", new TreeScalar(expected.name));

        var sub = new TreeCollection(4);
        int i = 0;
        sub.InsertChild(i, new TreeScalar(expected.subCounts[i++].ToString()));
        sub.InsertChild(i, new TreeScalar(expected.subCounts[i++].ToString()));
        sub.InsertChild(i, new TreeScalar(expected.subCounts[i++].ToString()));
        sub.InsertChild(i, new TreeScalar(expected.subCounts[i++].ToString()));
        root.InsertChild("subCounts", sub);
        
        var parser = EnumeratorParser.FromTree(root);
        var result = _deserializer.Deserialize<ValueStruct>(parser);
        
        Assert.True(expected.Equals(result));
    }

    private struct ValueStruct : IEquatable<ValueStruct> {
        public int amount;
        public string name;

        public int[] subCounts;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ValueStruct other && Equals(other);
        }

        public bool Equals(ValueStruct other) {
            return amount == other.amount && name == other.name && subCounts.SequenceEqual(other.subCounts);
        }

        public override int GetHashCode() {
            //Should be unused
            return 0;
        }
    }
}