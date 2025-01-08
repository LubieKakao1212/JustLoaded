using System.Globalization;

namespace JustLoaded.Logger;

public static class LogHelper {
    
    public static string StampMessage(this string message, LogLevel level) {
        return message
            .PrefixMessage(level.ToPrefix(), " ")
            .PrefixMessage(DateTime.Now.ToString(CultureInfo.CurrentCulture));
    }
    
    /// <param name="message"></param>
    /// <param name="prefix">If null, does not append anything</param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string PrefixMessage(this string message, string? prefix, string separator = "") {
        if (prefix == null) {
            return message;
        }
        return $"[{prefix}]{separator}{message}";
    }
    
}