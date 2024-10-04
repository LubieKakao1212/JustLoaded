using JustLoaded.Util.Validation;
using PathLib;

namespace JustLoaded.Filesystem;

public static class FilesystemUtil {

    public static IPurePath CollapsePath(this IPurePath path) {
        //TODO
        //path.Matches(purePath => !purePath.IsAbsolute(), () => new ());
        path = path.Relative();
        
        var parts = new List<string>(path.Parts);
        var stack = new Stack<string>(parts.Count);
        
        var upCount = 0;

        foreach (var part in parts) {
            if (part.Equals("..")) {
                if (!stack.TryPop(out _)) {
                    upCount++;
                }
            }
            else {
                stack.Push(part);
            }
        }
        
        if (!stack.TryPop(out var first)) {
            switch (upCount) {
                case 0:
                    return new PurePosixPath("");
                case 1:
                    return new PurePosixPath("..");
                default:
                    return new PurePosixPath(Enumerable.Repeat("..", upCount - 1).MergePathsReverse(".."));
            }
        }
        
        return new PurePosixPath(stack.Concat(Enumerable.Repeat("..", upCount)).MergePathsReverse(first));
    }

    public static IPurePath CollapseAbsolutePath(this IPurePath path) {
        path = path.CollapsePath();
        path.Dirname.StartsWith("..").Matches(b => !b, () => new DirectoryNotFoundException());
        return path;
    }
    
    /*public static string CollapsePath(this string path) {
        if (path == ".") {
            return "";
        }
        if (path.StartsWith("./")) {
            path = path.Substring(2);
        }
        
        var nodes = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

        var stack = new Stack<string>();

        var upCount = 0;
        
        foreach (var element in nodes) {
            if (element.Equals("..")) {
                if (!stack.TryPop(out var _)) {
                    upCount++;
                }
            }
            else {
                stack.Push(element);
            }
        }

        if (!stack.TryPop(out var first)) {
            switch (upCount) {
                case 0:
                    return "";
                case 1:
                    return "..";
                default:
                    return Enumerable.Repeat("..", upCount - 1).MergePathsReverse("..");
            }
        }
        
        return stack.Concat(Enumerable.Repeat("..", upCount)).MergePathsReverse(first);
    }
    
    public static string CollapseAbsolutePath(this string path) {
        path = CollapsePath(path);

        if (path.Equals("..") || path.Length >= 3 && path.Substring(0, 3).Equals("../")) {
            throw new DirectoryNotFoundException("Cannot escape root path!");
        }

        return path;
    }

    public static string CollapseAbsoluteFilePath(this string filePath) {
        if (filePath.EndsWith(".")) {
            throw new FileNotFoundException("File name cannot end with \".\"");
        }

        return CollapseAbsolutePath(filePath);
    }*/
    
    public static string MergePathsReverse(this IEnumerable<string> elements, string first) {
        return elements.Aggregate(first, (acc, str) => str + '/' + acc);
    }
    
    public static bool MatchPattern(this string str, string pattern) {
        var segments = pattern.Split('*');
        foreach (var segment in segments) {
            //var segment = p.Replace("*", "");
            var location = str.IndexOf(segment, StringComparison.Ordinal);
            if (location < 0) {
                return false;
            }
            str = str.Substring(location + segment.Length);
        }
        return true;
    }
}