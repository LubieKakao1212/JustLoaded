using PathLib;

namespace JustLoaded.Filesystem;

public static class PathExtensions
{

    public static readonly IPurePath Local = new PosixPath(".");
    public static readonly IPurePath CurrentDirectory = new PurePosixPath(Paths.CurrentDirectory.ToPosix());

    public static IPurePath AsPath(this string raw)
    {
        return new PosixPath(raw);
    }

    public static IPurePath RelativeToFixed(this IPurePath path, IPurePath relativeTo)
    {
        var partsBase = new List<string>(path.Parts);
        var partsRelativeTo = new List<string>(relativeTo.Parts);
        //Capacity is upper limit
        var partsOut = new List<string>(partsBase.Count + partsRelativeTo.Count);

        var matchIdx = partsBase[0].Equals(".") ? 1 : 0;
        var baseCount = partsBase.Count;
        var flag = matchIdx >= baseCount;

        var s = partsRelativeTo[0].Equals(".") ? 1 : 0;

        for (var i = s; i < partsRelativeTo.Count; i++)
        {
            if (!flag)
            {
                matchIdx++;
                if (i >= baseCount || !partsRelativeTo[i].Equals(partsBase[i]))
                {
                    flag = true;
                    matchIdx--;
                }
            }

            if (flag)
            {
                partsOut.Add("..");
            }
        }

        for (var i = matchIdx; i < partsBase.Count; i++)
        {
            partsOut.Add(partsBase[i]);
        }

        return new PosixPath(partsOut.ToArray());
    }
}
