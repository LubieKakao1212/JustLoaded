using System.Diagnostics.CodeAnalysis;
using JustLoaded.Filesystem;
using PathLib;

namespace JustLoaded.FileSystem.Tests.Relative.Simple;

//TODO Write RelativeFilesystem test cases for both simple and combined
public class SimpleRelativeFilesystemTests : RelativeFilesystemTester<Source>
{

    [NotNull]
    private VirtualFilesystem? Vfs { get; set; }

    protected override RelativeFilesystem SetupFilesystem()
    {
        Vfs = new VirtualFilesystem();

        return new RelativeFilesystem(Vfs, "prefix".AsPath());
    }

    protected override void MakeFile(IPurePath fileName, string content)
    {
        Vfs.AddFile(fileName, content);
    }
}

public class Source : RelativeFilesystemTestSourceBase
{

    public override IEnumerable<(IPurePath file, IPurePath query)> GetSingleFileRelativeSource
    {
        get
        {
            yield break;
        }
    }

    public override IEnumerable<(IPurePath query, IPurePath[] files, IPurePath[] results)> GetListDirsSource
    {
        get
        {
            yield break;
        }
    }

    public override IEnumerable<(IPurePath query, IPurePath[] files, IPurePath[] results)> GetListFilesShallowSource
    {
        get
        {
            yield break;
        }
    }

    public override IEnumerable<(IPurePath query, IPurePath[] files, IPurePath[] results)> GetListFilesRecursiveSource
    {
        get
        {
            yield break;
        }
    }

    public override IEnumerable<(string pattern, IPurePath[] files, IPurePath[] results)> GetListFilesPatternSource
    {
        get
        {
            yield break;
        }
    }
}
