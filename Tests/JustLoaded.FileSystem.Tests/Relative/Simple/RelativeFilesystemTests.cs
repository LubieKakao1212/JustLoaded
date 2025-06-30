using System.Diagnostics.CodeAnalysis;
using JustLoaded.Filesystem;

namespace JustLoaded.FileSystem.Tests.Relative.Simple;

//TODO Write RelativeFilesystem test cases for both simple and combined
public class SimpleRelativeFilesystemTests : RelativeFilesystemTester<Source> {

    [NotNull]
    private VirtualFilesystem? Vfs { get; set; }

    protected override RelativeFilesystem SetupFilesystem() {
        Vfs = new VirtualFilesystem();

        return new RelativeFilesystem(Vfs, "prefix".AsPath());
    }

    protected override void MakeFile(ModAssetPath fileName, string content) {
        Vfs.AddFile(fileName.path, content);
    }
}

public class Source : RelativeFilesystemTestSourceBase {

    public override IEnumerable<(ModAssetPath file, ModAssetPath query)> GetSingleFileRelativeSource {
        get {
            yield break;
        }
    }

    public override IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListDirsSource {
        get {
            yield break;
        }
    }

    public override IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesShallowSource {
        get {
            yield break;
        }
    }

    public override IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesRecursiveSource {
        get {
            yield break;
        }
    }

    public override IEnumerable<(string pattern, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesPatternSource {
        get {
            yield break;
        }
    }
}
