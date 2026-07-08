using System.Collections;
using JustLoaded.Filesystem;
using PathLib;
using static JustLoaded.FileSystem.Tests.IFilesystemTestSource;

namespace JustLoaded.FileSystem.Tests;

public interface IFilesystemTestSource
{

    public IEnumerable SourceSingleFileFlat => GetFileFlatFileName.Select(file => new TestCaseData(file));
    public IEnumerable SourceGetMultipleFiles => GetFilesFlatFileNames.Select(arr => new TestCaseData((IEnumerable<IPurePath>)arr));
    public IEnumerable SourceListDirs => GetListDirsSource.Select(MakeTestCase);
    public IEnumerable SourceListFilesShallow => GetListFilesShallowSource.Select(MakeTestCase);
    public IEnumerable SourceListFilesRecursive => GetListFilesRecursiveSource.Select(MakeTestCase);
    public IEnumerable SourceListFilesPattern => GetListFilesPatternSource.Select(MakeTestCase);

    public IEnumerable<IPurePath> GetFileFlatFileName { get; }
    public IEnumerable<IPurePath[]> GetFilesFlatFileNames { get; }

    protected IEnumerable<(IPurePath query, IPurePath[] files, IPurePath[] results)> GetListDirsSource { get; }
    protected IEnumerable<(IPurePath query, IPurePath[] files, IPurePath[] results)> GetListFilesShallowSource { get; }
    protected IEnumerable<(IPurePath query, IPurePath[] files, IPurePath[] results)> GetListFilesRecursiveSource { get; }
    protected IEnumerable<(string pattern, IPurePath[] files, IPurePath[] results)> GetListFilesPatternSource { get; }

    #region Utilities

    public static TestCaseData MakeTestCase<TQuery>((TQuery query, IPurePath[] files, IPurePath[] results) source)
    {
        return new TestCaseData(source.query, source.files, source.results);
    }

    public static IPurePath[] PathsWithBlankSource(params string[] paths)
    {
        return paths.Select(s => s.AsPath()).ToArray();
    }

    #endregion

}

public class FilesystemTestSource : IFilesystemTestSource
{

    public IEnumerable<IPurePath> GetFileFlatFileName => PathsWithBlankSource("coolFile", "dir/coolFile", "dir1/coolFile");

    public IEnumerable<IPurePath[]> GetFilesFlatFileNames =>
    [
        PathsWithBlankSource("file1", "file2"),
        PathsWithBlankSource("dir/file1", "file2"),
        PathsWithBlankSource("file1", "dir/file2"),
        PathsWithBlankSource("dir/file1", "dir/file2")
    ];

    public IEnumerable<(IPurePath query, IPurePath[] files, IPurePath[] results)> GetListDirsSource
    {
        get
        {
            yield return ("./".AsPath(), PathsWithBlankSource("dir/file1", "file2"), PathsWithBlankSource("dir"));
            yield return ("./".AsPath(), PathsWithBlankSource("dir/file1", "dir/file2"), PathsWithBlankSource("dir"));
            yield return ("./".AsPath(), PathsWithBlankSource("dir/file1", "dir2/file2"), PathsWithBlankSource("dir", "dir2"));
            yield return ("./a".AsPath(), PathsWithBlankSource("a/dir/file1", "a/dir2/file2"), PathsWithBlankSource("a/dir", "a/dir2"));
            yield return ("./a/".AsPath(), PathsWithBlankSource("a/dir/file1", "a/dir2/file2"), PathsWithBlankSource("a/dir", "a/dir2"));
            yield return ("./a/".AsPath(), PathsWithBlankSource("a/dir/file1", "a/file2"), PathsWithBlankSource("a/dir"));
            yield return ("./a/b".AsPath(), PathsWithBlankSource("a/b/file1", "a/c/file2", "a/b/dir/file2"), PathsWithBlankSource("a/b/dir"));
        }
    }

    public IEnumerable<(IPurePath query, IPurePath[] files, IPurePath[] results)> GetListFilesShallowSource
    {
        get
        {
            yield return ("./".AsPath(), PathsWithBlankSource("dir/file1", "file2"), PathsWithBlankSource("file2"));
            yield return ("./".AsPath(), PathsWithBlankSource("dir/file1", "dir/file2"), PathsWithBlankSource());
            yield return ("./dir".AsPath(), PathsWithBlankSource("dir/file1", "dir2/file2"), PathsWithBlankSource("dir/file1")); //Duplicate?
            yield return ("./a/".AsPath(), PathsWithBlankSource("a/dir/file1", "a/dir2/file2"), PathsWithBlankSource());
            yield return ("./a/dir".AsPath(), PathsWithBlankSource("a/dir/file1", "a/dir/file2"), PathsWithBlankSource("a/dir/file1", "a/dir/file2"));
            yield return ("./a/".AsPath(), PathsWithBlankSource("a/dir/file1", "a/file2"), PathsWithBlankSource("a/file2"));
        }
    }

    public IEnumerable<(IPurePath query, IPurePath[] files, IPurePath[] results)> GetListFilesRecursiveSource
    {
        get
        {
            yield return (".".AsPath(), PathsWithBlankSource("dir/file1", "file2"), PathsWithBlankSource("file2", "dir/file1"));
            yield return ("dir".AsPath(), PathsWithBlankSource("dir/file1", "dir2/file2"), PathsWithBlankSource("dir/file1"));
            yield return (".".AsPath(), PathsWithBlankSource("dir/dir1/dir2/file1"), PathsWithBlankSource("dir/dir1/dir2/file1"));
        }
    }

    public IEnumerable<(string pattern, IPurePath[] files, IPurePath[] results)> GetListFilesPatternSource
    {
        get
        {
            yield return ("*", PathsWithBlankSource("file.json", "file.txt", "file", "apple.txt"),
                PathsWithBlankSource("file.json", "file.txt", "file", "apple.txt"));

            yield return ("*.*", PathsWithBlankSource("file.json", "file.txt", "apple.txt"),
                PathsWithBlankSource("file.json", "file.txt", "apple.txt"));

            yield return ("*.txt", PathsWithBlankSource("file.json", "file.txt", "file", "apple.txt"),
                PathsWithBlankSource("file.txt", "apple.txt"));

            yield return ("file.*", PathsWithBlankSource("file.json", "file.txt", "file", "apple.txt"),
                PathsWithBlankSource("file.json", "file.txt"));
        }
    }
}
