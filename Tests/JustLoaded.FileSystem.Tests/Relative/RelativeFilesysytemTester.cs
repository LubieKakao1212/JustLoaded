using System.Collections;
using JustLoaded.Filesystem;
using PathLib;

namespace JustLoaded.FileSystem.Tests.Relative;

public abstract class RelativeFilesystemTester<TSource> : FilesystemTester<RelativeFilesystem, TSource> where TSource : IRelativeFilesystemTestSource, new()
{
    protected static IEnumerable SourceSingleFileRelative => new TSource().SourceSingleFileRelative;

    [TestCaseSource(nameof(SourceSingleFileRelative))]
    public void GetSingleFileRelative(IPurePath file, IPurePath query)
    {
        const string fileContent = "A cool file content";
        MakeFile(file, fileContent);

        using var stream = fs.OpenFile(query);
        Assert.That(stream, Is.Not.Null);

        using var text = new StreamReader(stream);
        Assert.That(text.ReadLine(), Is.EqualTo(fileContent));
    }

    public override void GetSingleFile(IPurePath fileName)
    {
        Assert.Pass("Not applicable");
    }

    public override void AddGetMultiple(IEnumerable<IPurePath> fileNames)
    {
        Assert.Pass("Not applicable");
    }
}

public interface IRelativeFilesystemTestSource : IFilesystemTestSource
{

    public IEnumerable SourceSingleFileRelative => GetSingleFileRelativeSource.Select(file => new TestCaseData(file.file, file.query));
    public IEnumerable<(IPurePath file, IPurePath query)> GetSingleFileRelativeSource { get; }
}

public abstract class RelativeFilesystemTestSourceBase : IRelativeFilesystemTestSource
{
    public IEnumerable<IPurePath> GetFileFlatFileName => Enumerable.Repeat(string.Empty.AsPath(), 1);
    public IEnumerable<IPurePath[]> GetFilesFlatFileNames => Enumerable.Repeat(new[] { string.Empty.AsPath() }, 1);

    public abstract IEnumerable<(IPurePath file, IPurePath query)> GetSingleFileRelativeSource { get; }
    public abstract IEnumerable<(IPurePath query, IPurePath[] files, IPurePath[] results)> GetListDirsSource { get; }
    public abstract IEnumerable<(IPurePath query, IPurePath[] files, IPurePath[] results)> GetListFilesShallowSource { get; }
    public abstract IEnumerable<(IPurePath query, IPurePath[] files, IPurePath[] results)> GetListFilesRecursiveSource { get; }
    public abstract IEnumerable<(string pattern, IPurePath[] files, IPurePath[] results)> GetListFilesPatternSource { get; }
}
