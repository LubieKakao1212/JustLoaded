namespace JustLoaded.Logger;

public enum LogLevel {
    None,
    Info,
    Warning,
    Error
}

[Flags]
public enum LogFilter {
    Always = 0,
    Debug = 1
}

public static class MessageHelper {

    public static string? ToPrefix(this LogLevel level) {
        if (level == LogLevel.None) {
            return null;
        }
        return level.ToString();
    }
}