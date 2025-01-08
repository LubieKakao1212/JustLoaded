namespace JustLoaded.Logger;

public class Logger : LoggerBase {
    public Logger(params ILogModule[] modules) : base(modules) {
    }

    public override void Log(string message, LogLevel level, LogFilter filter) {
        if (!filterFlags.Contains(filter)) {
            return;
        }
        foreach (var module in modules) {
            module.ProcessMessage(message, level, filter);
        }
    }
    
}