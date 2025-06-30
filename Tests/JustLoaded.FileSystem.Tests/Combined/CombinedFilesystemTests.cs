using JustLoaded.Filesystem;

namespace JustLoaded.FileSystem.Tests.Combined;

public class CombinedFilesystemTests : FilesystemTester<CombinedFilesystem, CfsTestSourceSource> {

    private VirtualFilesystem[]? _nestedFilesystems;
    
    protected override CombinedFilesystem SetupFilesystem() {
        var modCount = 3;

        var cfs = new CombinedFilesystem();

        _nestedFilesystems = new VirtualFilesystem[modCount];
        
        for (int i = 0; i < modCount; i++) {
            _nestedFilesystems[i] = new VirtualFilesystem();
            cfs.AddFileSystem(i.ToString(), _nestedFilesystems[i]);
        }

        return cfs;
    }

    protected override void MakeFile(ModAssetPath fileName, string content) {
        _nestedFilesystems?[int.Parse(fileName.modSelector)].AddFile(fileName.path, content);
    }
    
}

public class CfsTestSourceSource : IFilesystemTestSource {

    private static readonly string[] simpleFiles = { "coolFile", "dir/coolFile", "dir1/coolFile" };
    
    public IEnumerable<ModAssetPath> GetFileFlatFileName =>
        TestSourcesUtil.PathsFromCommonMod("0", simpleFiles)
            .Concat(TestSourcesUtil.PathsFromCommonMod("1", simpleFiles))
            .Concat(TestSourcesUtil.PathsFromCommonMod("2", simpleFiles));

    public IEnumerable<ModAssetPath[]> GetFilesFlatFileNames => Enumerable.Repeat(Paths((0, "dummy")), 1);

    public IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListDirsSource {
        get {
            /*
            yield return ("./".BlankMod(), PathsWithBlankSource("dir/file1", "file2"), PathsWithBlankSource("dir"));
            yield return ("./".BlankMod(), PathsWithBlankSource("dir/file1", "dir/file2"), PathsWithBlankSource("dir"));
            yield return ("./".BlankMod(), PathsWithBlankSource("dir/file1", "dir2/file2"), PathsWithBlankSource("dir", "dir2"));
            yield return ("./a".BlankMod(), PathsWithBlankSource( "a/dir/file1", "a/dir2/file2"), PathsWithBlankSource("a/dir", "a/dir2"));
            yield return ("./a/".BlankMod(), PathsWithBlankSource( "a/dir/file1", "a/dir2/file2"), PathsWithBlankSource("a/dir", "a/dir2"));
            yield return ("./a/".BlankMod(), PathsWithBlankSource( "a/dir/file1", "a/file2"),  PathsWithBlankSource("a/dir"));
            yield return ("./a/b".BlankMod(), PathsWithBlankSource( "a/b/file1", "a/c/file2", "a/b/dir/file2"), PathsWithBlankSource("a/b/dir"));
            */
            yield return (Path((0, "./")), 
                Paths(
                    (0,"dir/file"), 
                    (1,"dir/file"),
                    (2,"dir/file"),
                    (2,"dir2/file")), 
                Paths(
                    (0, "dir")
                ));
            
            yield return (Path((1, "./")), 
                Paths(
                    (0,"dir/file"), 
                    (1,"dir/file"),
                    (2,"dir/file"),
                    (2,"dir2/file")), 
                Paths(
                    (1, "dir")
                ));
            
            
            yield return (Path((2, "./")), 
                Paths(
                    (0,"dir/file"), 
                    (1,"dir/file"),
                    (2,"dir/file"),
                    (2,"dir2/file")), 
                Paths(
                    (2, "dir"),
                    (2, "dir2")
                    ));
            
            yield return (QueryAny("./"), 
                Paths(
                    (0,"dir/file"), 
                    (1,"dir/file"),
                    (2,"dir/file"),
                    (2,"dir2/file1")), 
                Paths(
                    (0, "dir"),
                    (1, "dir"),
                    (2, "dir"),
                    (2, "dir2")
                ));

            yield return (QueryAny("./dir"), 
                Paths(
                    (0,"dir/dir1/file"), 
                    (1,"dir/dir2/file"),
                    (2,"dir/dir3/file"),
                    (2,"dir/dir4/file1")), 
                Paths(
                    (0, "dir/dir1"),
                    (1, "dir/dir2"),
                    (2, "dir/dir3"),
                    (2, "dir/dir4")
                ));
        }
    }
    
    public IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesShallowSource {
        get {
            (ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results) MakeCase(ModAssetPath query, ModAssetPath[] results) {
                return (query,
                    Paths(
                        //In root
                        (0, "file"),
                        (1, "file"),
                        //(2, "file"), Missing in 2
                        //In directory
                        (0, "dir/file"),
                        (1, "dir/file1"),
                        (2, "dir/file"),
                        
                        //Two files for mod 1 in dir2
                        (0, "dir2/file"),
                        (1, "dir2/file1"),
                        (1, "dir2/file"),
                        (2, "dir2/file"),
                        //nested dirs
                        (0, "dir/dir2/file"),
                        (1, "dir/dir2/file"),
                        (2, "dir/dir2/file")
                        ),
                    results);
            }

            IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> MakeCases(string queryPath, ModAssetPath[] results) {
                return MakeCasesBase(MakeCase, queryPath, results);
            }

            return MakeCases("./",
                Paths(
                    (0, "file"),
                    (1, "file")
                    ))
                .Concat(MakeCases("./dir",
                    Paths(
                        (0, "dir/file"),
                        (1, "dir/file1"),
                        (2, "dir/file")
                    )))
                .Concat(MakeCases("./dir2",
                    Paths(
                        (0, "dir2/file"),
                        (1, "dir2/file1"),
                        (1, "dir2/file"),
                        (2, "dir2/file")
                        )))
                .Concat(MakeCases("./dir/dir2",
                     Paths(
                         (0, "dir/dir2/file"),
                         (1, "dir/dir2/file"),
                         (2, "dir/dir2/file")
                         )));
        }
    }

    public IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesRecursiveSource {
        get {
            (ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results) MakeCase(ModAssetPath query, ModAssetPath[] results) {
                return (query,
                    Paths(
                        //In root
                        (0, "file"),
                        (1, "file"),
                        //(2, "file"), Missing in 2
                        //In directory (one is different)
                        (0, "dir/file"),
                        (1, "dir/file1"),
                        (2, "dir/file"),
                        
                        //Two files for mod 1 in dir2
                        (0, "dir2/file"),
                        (1, "dir2/file1"),
                        (1, "dir2/file"),
                        (2, "dir2/file"),
                        //nested dirs
                        (0, "dir/dir2/file"),
                        (1, "dir/dir2/file"),
                        (2, "dir/dir2/file")
                    ),
                    results);
            }

            IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> MakeCases(string queryPath, ModAssetPath[] results) {
                return MakeCasesBase(MakeCase, queryPath, results); 
            }

            return MakeCases("dir2",
                    Paths(
                        (0, "dir2/file"),
                        (1, "dir2/file1"),
                        (1, "dir2/file"),
                        (2, "dir2/file")
                    ))
                .Concat(MakeCases("dir",
                    Paths(
                        (0, "dir/file"),
                        (1, "dir/file1"),
                        (2, "dir/file"),
                        (0, "dir/dir2/file"),
                        (1, "dir/dir2/file"),
                        (2, "dir/dir2/file")
                    )))
                .Concat(MakeCases(".",
                    Paths(
                        //Everything
                        (0, "file"),
                        (1, "file"),
                        (0, "dir/file"),
                        (1, "dir/file1"),
                        (2, "dir/file"),
                        (0, "dir2/file"),
                        (1, "dir2/file1"),
                        (1, "dir2/file"),
                        (2, "dir2/file"),
                        (0, "dir/dir2/file"),
                        (1, "dir/dir2/file"),
                        (2, "dir/dir2/file")
                        )));
        }
    }

    public IEnumerable<(string pattern, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesPatternSource {
        get {
            yield return ("*", Paths((0, "dummy")), Paths((0, "dummy")));
        }
    }

    public static ModAssetPath QueryAny(string path) {
        return path.AsPath().FromAnyMod();
    }
    
    public static ModAssetPath Path((int mod, string path) path) {
        return path.path.AsPath().FromMod(path.mod.ToString());
    }

    public static ModAssetPath[] Paths(params (int mod, string path)[] paths) {
        return paths.Select(Path).ToArray();
    }
    
    private static IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> MakeCasesBase(Func<ModAssetPath, ModAssetPath[], (ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> MakeCase, string queryPath, ModAssetPath[] results) {
        yield return MakeCase(Path((0, queryPath)), results.Where(path => path.modSelector == "0").ToArray());
        yield return MakeCase(Path((1, queryPath)), results.Where(path => path.modSelector == "1").ToArray());
        yield return MakeCase(Path((2, queryPath)), results.Where(path => path.modSelector == "2").ToArray());
        yield return MakeCase(QueryAny(queryPath), results);
    }
    
}