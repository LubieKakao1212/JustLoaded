using System.Collections;
using JustLoaded.Filesystem;
using PathLib;

namespace JustLoaded.FileSystem.Tests;

public abstract class FilesystemTester<TFilesystem, TSource> where TSource : IFilesystemTestSource, new() where TFilesystem : IFilesystem
{
    protected TFilesystem fs;

    protected static IEnumerable SourceSingleFileFlat => new TSource().SourceSingleFileFlat;
    protected static IEnumerable SourceMultipleFilesFlat => new TSource().SourceGetMultipleFiles;

    protected static IEnumerable SourceListDirs => new TSource().SourceListDirs;
    protected static IEnumerable SourceListFilesShallow => new TSource().SourceListFilesShallow;
    protected static IEnumerable SourceListFilesRecursive => new TSource().SourceListFilesRecursive;
    protected static IEnumerable SourceListFilesPattern => new TSource().SourceListFilesPattern;

    [SetUp]
    public void Setup()
    {
        fs = SetupFilesystem();
    }

    [TestCaseSource(nameof(SourceSingleFileFlat))]
    public virtual void GetSingleFile(IPurePath fileName)
    {
        const string fileContent = "A Cool File Content";

        MakeFile(fileName, fileContent);

        using var stream = fs.OpenFile(fileName);
        Assert.That(stream, Is.Not.Null);

        using var text = new StreamReader(stream);
        Assert.That(text.ReadLine(), Is.EqualTo(fileContent));
    }

    [TestCaseSource(nameof(SourceMultipleFilesFlat))]
    public virtual void AddGetMultiple(IEnumerable<IPurePath> fileNames)
    {
        var files = fileNames.ToArray();

        for (var i = 0; i < files.Length; i++)
        {
            MakeFile(files[i], i.ToString());
        }

        for (var i = 0; i < files.Length; i++)
        {
            AssertFileContents(fs, files[i], i.ToString());
        }
    }

    [TestCaseSource(nameof(SourceListDirs))]
    public void ListPaths(IPurePath listDir, IPurePath[] files, IPurePath[] expectedDirs)
    {
        foreach (var file in files)
        {
            MakeFile(file, file.ToString());
        }

        var dirsSet = new HashSet<string>(expectedDirs.Select(p => p.ToPosix()));
        foreach (var dir in fs.ListPaths(listDir))
        {
            Assert.That(dirsSet, Contains.Item(dir.ToPosix()));
            dirsSet.Remove(dir.ToPosix());
        }

        Assert.That(dirsSet, Is.Empty);
    }

    [TestCaseSource(nameof(SourceListFilesShallow))]
    public void ListFilesShallow(IPurePath listDir, IPurePath[] files, IPurePath[] expectedFiles)
    {
        DoListFiles(listDir, files, expectedFiles, "*", false);
    }

    [TestCaseSource(nameof(SourceListFilesRecursive))]
    public void ListFilesRecursive(IPurePath listDir, IPurePath[] files, IPurePath[] expectedFiles)
    {
        DoListFiles(listDir, files, expectedFiles, "*", true);
    }

    [TestCaseSource(nameof(SourceListFilesPattern))]
    public void ListFilesPattern(string pattern, IPurePath[] files, IPurePath[] expectedFiles)
    {
        DoListFiles(PathExtensions.Local, files, expectedFiles, pattern, false);
    }

    protected abstract TFilesystem SetupFilesystem();

    protected abstract void MakeFile(IPurePath fileName, string content);

    private void DoListFiles(IPurePath listDir, IPurePath[] files, IPurePath[] expectedFiles, string pattern, bool recursive)
    {
        foreach (var file in files)
        {
            MakeFile(file, file.ToString());
        }

        var dirsSet = new HashSet<string>(expectedFiles.Select(p => p.ToPosix()));
        foreach (var dir in fs.ListFiles(listDir, pattern, recursive))
        {
            Assert.That(dirsSet, Contains.Item(dir.ToPosix()));

            dirsSet.Remove(dir.ToPosix());
        }
        Assert.That(dirsSet, Is.Empty);
    }

    private static void AssertFileContents(TFilesystem vfs, IPurePath file, string expectedContent)
    {
        using var stream1 = vfs.OpenFile(file);
        Assert.That(stream1, Is.Not.Null);
        using var text = new StreamReader(stream1);
        Assert.That(text.ReadLine(), Is.EqualTo(expectedContent));
    }
}
