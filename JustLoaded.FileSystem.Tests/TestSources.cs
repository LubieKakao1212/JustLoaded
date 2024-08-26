using System.Collections;
using System.Threading.Tasks.Sources;
using JustLoaded.Content;
using JustLoaded.Filesystem;

namespace JustLoaded.FileSystem.Tests;

public interface IFilesystemTestSource {

    public IEnumerable SourceSingleFileFlat => GetFileFlatFileName.Select(file => new TestCaseData(file));
    public IEnumerable SourceGetMultipleFiles => GetFilesFlatFileNames.Select(arr=> new TestCaseData((IEnumerable<string>)arr));
    public IEnumerable SourceListDirs => GetListDirsQueryPath.Zip(GetListDirsFiles, GetListDirsExpectedResult).Select(elements=> new TestCaseData(elements.First, elements.Second, elements.Third));
    public IEnumerable SourceListFilesShallow => GetListFilesShallowQuery.Zip(GetListFilesShallowFiles, GetListFilesShallowExpectedResult).Select(elements=> new TestCaseData(elements.First, elements.Second, elements.Third));
    public IEnumerable SourceListFilesRecursive => GetListFilesRecursiveQuery.Zip(GetListFilesRecursiveFiles, GetListFilesRecursiveExpectedResult).Select(elements=> new TestCaseData(elements.First, elements.Second, elements.Third));
    
    
    public IEnumerable<string> GetFileFlatFileName { get; }
    public IEnumerable<string[]> GetFilesFlatFileNames { get; }
    
    
    public IEnumerable<string> GetListDirsQueryPath { get; }
    public IEnumerable<string[]> GetListDirsFiles { get; }
    public IEnumerable<ContentKey[]> GetListDirsExpectedResult { get; }
    
    
    public IEnumerable<string> GetListFilesShallowQuery { get; }
    public IEnumerable<string[]> GetListFilesShallowFiles { get; }
    public IEnumerable<ContentKey[]> GetListFilesShallowExpectedResult { get; }
    
    public IEnumerable<string> GetListFilesRecursiveQuery { get; }
    public IEnumerable<string[]> GetListFilesRecursiveFiles { get; }
    public IEnumerable<ContentKey[]> GetListFilesRecursiveExpectedResult { get; }

    protected IEnumerable<(string query, string[] files, ContentKey[] results)> GetListFiesRecursiveSource { get; }

    public static TestCaseData MakeTestCase((string query, string[] files, ContentKey[] results) source) {
        return new TestCaseData(source.query, source.files, source.results);
    }
}

public class FilesystemTestSource : IFilesystemTestSource {
    
    public virtual IEnumerable<string> GetFileFlatFileName => new []{"coolFile", "dir/coolFile", "dir1/coolFile"};

    public virtual IEnumerable<string[]> GetFilesFlatFileNames => new[] {
        new [] { "file1", "file2" },
        new [] { "dir/file1", "file2" },
        new [] { "file1", "dir/file2" },
        new [] { "dir/file1", "dir/file2" }
    };

    public IEnumerable<string> GetListDirsQueryPath => new[] { "./", "./", "./", "./a", "./a/", "./a/", "./a/b" };
    
    public IEnumerable<string[]> GetListDirsFiles => new[] {
        new[] { "dir/file1", "file2"},
        new[] { "dir/file1", "dir/file2"},
        new[] { "dir/file1", "dir2/file2"},
        new[] { "a/dir/file1", "a/dir2/file2"},
        new[] { "a/dir/file1", "a/dir2/file2"},
        new[] { "a/dir/file1", "a/file2"},
        new[] { "a/b/file1", "a/c/file2", "a/b/dir/file2"}
    };

    public IEnumerable<ContentKey[]> GetListDirsExpectedResult =>
        new[] {
            new[] { "dir" },
            new[] { "dir" },
            new[] { "dir", "dir2" },
            new[] { "a/dir", "a/dir2" },
            new[] { "a/dir", "a/dir2" },
            new[] { "a/dir" },
            new[] { "a/b/dir" }
        }.Select(arr => arr.Select(str => new ContentKey("", str)).ToArray());

    public IEnumerable<string> GetListFilesShallowQuery => new[] {
        "./",
        "./",
        "./dir",
        "./a/",
        "./a/dir",
        "./a/"
    };

    public IEnumerable<string[]> GetListFilesShallowFiles => new[] {
        new[] { "dir/file1", "file2" },
        new[] { "dir/file1", "dir/file2" },
        new[] { "dir/file1", "dir2/file2" },
        new[] { "a/dir/file1", "a/dir2/file2" },
        new[] { "a/dir/file1", "a/dir/file2" },
        new[] { "a/dir/file1", "a/file2" }
    };
    public IEnumerable<ContentKey[]> GetListFilesShallowExpectedResult => 
        new [] {
            new[] { "file2" },
            new string[] { },
            new[] { "dir/file1" },
            new string[] { },
            new[] { "a/dir/file1", "a/dir/file2" },
            new[] { "a/file2" }
    }.Select(arr => arr.Select(str => new ContentKey("", str)).ToArray());
    
}