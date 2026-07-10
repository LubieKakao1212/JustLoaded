using JustLoaded.Serialization.Tree;
using JustLoaded.Serialization.Yaml;
using YamlDotNet.Serialization;

namespace JustLoaded.Serialization.Tests;

public class Yaml2Tree {

    private static readonly IDeserializer Deserializer = new DeserializerBuilder()
        .WithTreeDeserialization()
        .Build();
    
/*
ala:
  - aaa
  - bbb
  - ccc
ma: 2137
kota:
  ma: 2
  ale: abc
*/
    [Fact]
    public void Test() {
        var yaml = "ala:\n  - aaa\n  - bbb\n  - ccc\nma: 2137\nkota:\n  ma: 2\n  ale: abc";
        var node = Deserializer.Deserialize<ITreeNode>(yaml);

        var root = Assert.IsAssignableFrom<ITreeMapping>(node);
        var ala = Assert.IsAssignableFrom<ITreeCollection>(root.GetChild("ala"));

        var content1 = new string[] { "aaa", "bbb", "ccc" };
        Assert.True(ala.Count == content1.Length);
        for (int i = 0; i < ala.Count; i++) {
            Assert.Equal(content1[i], Assert.IsAssignableFrom<ITreeScalar>(ala[i]).Value);
        }
        var ma = Assert.IsAssignableFrom<ITreeScalar>(root.GetChild("ma"));
        Assert.Equal("2137", ma.Value);
        var kota = Assert.IsAssignableFrom<ITreeMapping>(root.GetChild("kota"));
        var kotama = Assert.IsAssignableFrom<ITreeScalar>(kota.GetChild("ma"));
        Assert.Equal("2", kotama.Value);
        var kotaale = Assert.IsAssignableFrom<ITreeScalar>(kota.GetChild("ale"));
        Assert.Equal("abc", kotaale.Value);
    }
}