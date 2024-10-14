using System.Collections;
using System.Threading.Tasks.Sources;
using JustLoaded.Content;
using JustLoaded.Filesystem;
using static JustLoaded.FileSystem.Tests.IFilesystemTestSource;

namespace JustLoaded.FileSystem.Tests;

public interface IFilesystemTestSource {

    public IEnumerable SourceSingleFileFlat => GetFileFlatFileName.Select(file => new TestCaseData(file));
    public IEnumerable SourceGetMultipleFiles => GetFilesFlatFileNames.Select(arr=> new TestCaseData((IEnumerable<ModAssetPath>)arr));
    public IEnumerable SourceListDirs => GetListDirsSource.Select(MakeTestCase);
    public IEnumerable SourceListFilesShallow => GetListFilesShallowSource.Select(MakeTestCase);
    public IEnumerable SourceListFilesRecursive => GetListFilesRecursiveSource.Select(MakeTestCase);
    public IEnumerable SourceListFilesPattern => GetListFilesPatternSource.Select(MakeTestCase);
    
    public IEnumerable<ModAssetPath> GetFileFlatFileName { get; }
    public IEnumerable<ModAssetPath[]> GetFilesFlatFileNames { get; }
    
    protected IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListDirsSource { get; }
    protected IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesShallowSource { get; }
    protected IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesRecursiveSource { get; }
    protected IEnumerable<(string pattern, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesPatternSource { get; }
    
    #region Utilities
    public static TestCaseData MakeTestCase<TQuery>((TQuery query, ModAssetPath[] files, ModAssetPath[] results) source) {
        return new TestCaseData(source.query, source.files, source.results);
    }

    public static ModAssetPath[] PathsWithBlankSource(params string[] paths) {
        return paths.Select(s => s.BlankMod()).ToArray();
    }

    public static ModAssetPath[] Keys(params (string source, string path)[] raw) => raw.Select(rawKey => rawKey.path.AsPath().FromMod(rawKey.source)).ToArray();
    #endregion
}

public class FilesystemTestSource : IFilesystemTestSource {
    
    public IEnumerable<ModAssetPath> GetFileFlatFileName => PathsWithBlankSource("coolFile", "dir/coolFile", "dir1/coolFile");

    public IEnumerable<ModAssetPath[]> GetFilesFlatFileNames => new[] {
        PathsWithBlankSource("file1", "file2"),
        PathsWithBlankSource("dir/file1", "file2"),
        PathsWithBlankSource("file1", "dir/file2"),
        PathsWithBlankSource("dir/file1", "dir/file2")
    };

    public IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListDirsSource {
        get {
            yield return ("./".BlankMod(), PathsWithBlankSource("dir/file1", "file2"), PathsWithBlankSource("dir"));
            yield return ("./".BlankMod(), PathsWithBlankSource("dir/file1", "dir/file2"), PathsWithBlankSource("dir"));
            yield return ("./".BlankMod(), PathsWithBlankSource("dir/file1", "dir2/file2"), PathsWithBlankSource("dir", "dir2"));
            yield return ("./a".BlankMod(), PathsWithBlankSource( "a/dir/file1", "a/dir2/file2"), PathsWithBlankSource("a/dir", "a/dir2"));
            yield return ("./a/".BlankMod(), PathsWithBlankSource( "a/dir/file1", "a/dir2/file2"), PathsWithBlankSource("a/dir", "a/dir2"));
            yield return ("./a/".BlankMod(), PathsWithBlankSource( "a/dir/file1", "a/file2"),  PathsWithBlankSource("a/dir"));
            yield return ("./a/b".BlankMod(), PathsWithBlankSource( "a/b/file1", "a/c/file2", "a/b/dir/file2"), PathsWithBlankSource("a/b/dir"));
        }
    }

    public IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesShallowSource {
        get {
            yield return ("./".BlankMod(), PathsWithBlankSource("dir/file1", "file2"), PathsWithBlankSource("file2"));
            yield return ("./".BlankMod(), PathsWithBlankSource("dir/file1", "dir/file2"), PathsWithBlankSource());
            yield return ("./dir".BlankMod(), PathsWithBlankSource("dir/file1", "dir2/file2"), PathsWithBlankSource("dir/file1")); //Duplicate?
            yield return ("./a/".BlankMod(), PathsWithBlankSource("a/dir/file1", "a/dir2/file2"), PathsWithBlankSource());
            yield return ("./a/dir".BlankMod(), PathsWithBlankSource("a/dir/file1", "a/dir/file2"), PathsWithBlankSource("a/dir/file1", "a/dir/file2"));
            yield return ("./a/".BlankMod(), PathsWithBlankSource("a/dir/file1", "a/file2"), PathsWithBlankSource("a/file2"));
        }
    }
    
    public IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesRecursiveSource {
        get {
            yield return (".".BlankMod(), PathsWithBlankSource("dir/file1", "file2"), PathsWithBlankSource("file2", "dir/file1"));
            yield return ("dir".BlankMod(), PathsWithBlankSource("dir/file1", "dir2/file2"), PathsWithBlankSource("dir/file1"));
            yield return (".".BlankMod(), PathsWithBlankSource("dir/dir1/dir2/file1"), PathsWithBlankSource("dir/dir1/dir2/file1"));
        }
    }

    public IEnumerable<(string pattern, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesPatternSource {
        get {
            yield return ("*", PathsWithBlankSource("file.json", "file.txt", "file", "apple.txt"), 
                PathsWithBlankSource("file.json", "file.txt", "file", "apple.txt"));
            
            yield return ("*.*", PathsWithBlankSource("file.json", "file.txt", "apple.txt"), 
                PathsWithBlankSource("file.json", "file.txt", "apple.txt"));
            
            yield return ("*.txt",PathsWithBlankSource("file.json", "file.txt", "file", "apple.txt"), 
                PathsWithBlankSource("file.txt", "apple.txt"));
            
            yield return ("file.*", PathsWithBlankSource("file.json", "file.txt", "file", "apple.txt"), 
                PathsWithBlankSource("file.json", "file.txt"));
        }
    }
    
}