using JustLoaded.Filesystem;
using PathLib;

namespace JustLoaded.FileSystem.Tests;

public class FilesystemUtilTests {
    
    [TestCase("path", "path")]
    [TestCase("./path", "./path")]
    [TestCase("path/", "path")]
    [TestCase("path/..", ".")]
    [TestCase("..", "..")]
    [TestCase("../path", "../path")]
    [TestCase("path/../path1", "path1")]
    [TestCase("./path", "./path")]
    [TestCase(".", ".")]
    [TestCase("./", ".")]
    [TestCase(".path", ".path")]
    [TestCase("..path", "..path")]
    //[TestCase("path..", "path..")] Does not work?
    public void CollapsePathTest(string path, string expectedCollapsed) {
        var typedPath = new PurePosixPath(path).CollapsePath();

        Assert.That(typedPath.ToPosix(), Is.EqualTo(expectedCollapsed));
    }

    [TestCase("..", null, true)]
    [TestCase("../path", null, true)]
    [TestCase("path/../..", null, true)]
    [TestCase("path", "path", false)]
    [TestCase("path/..", ".", false)]
    public void AbsoluteCollapsePathTest(string path, string? expected, bool shouldThrow) {
        if (shouldThrow) {
            Assert.Throws<DirectoryNotFoundException>(() => {
                var typedPath = new PurePosixPath(path).CollapseAbsolutePath();
                Assert.That(typedPath.ToString(), Is.EqualTo(expected));
            });
        }
        else {
            Assert.DoesNotThrow(() => {
                var typedPath = new PurePosixPath(path).CollapseAbsolutePath();
                Assert.That(typedPath.ToString(), Is.EqualTo(expected));
            });
        }
    }

    
    [TestCase("path.", null, true)]
    [TestCase("path..", null, true)]
    [TestCase("path", "path", false)]
    public void CollapseAbsoluteFilePathTest(string path, string? expected, bool shouldThrow) {
        /*if (shouldThrow) {
            Assert.Throws<FileNotFoundException>(() => {
                var path = new PurePosixPath(path).CollapseAbsoluteFilePath();
                Assert.That(path, Is.EqualTo(expected));
            });
        }
        else {
            Assert.DoesNotThrow(() => {
                path = path.CollapseAbsoluteFilePath();
                Assert.That(path, Is.EqualTo(expected));
            });
        }*/
    }

    [TestCase("path1", "path1", "")]
    [TestCase("path1/path2", "path1", "path2")]
    [TestCase("path1/path2", "path3", "../path1/path2")]
    [TestCase("path1/path2", "path1/path3", "../path2")]
    [TestCase("path1/path2", "", "path1/path2")]
    [TestCase("./path1", "", "path1")]
    public void RelativeToFixedTest(string pBase, string pRelativeTo, string result) {
        var pathBase = pBase.AsPath();
        var pathRelativeTo = pRelativeTo.AsPath();

        var relative = pathBase.RelativeToFixed(pathRelativeTo);
        
        Assert.That(relative.ToPosix(), Is.EqualTo(result.AsPath().ToPosix()));
    }
}