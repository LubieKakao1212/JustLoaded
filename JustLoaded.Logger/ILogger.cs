using System.Diagnostics;
using System.Text;

namespace JustLoaded.Logger;

public interface ILogger {
    
    public void Log(string message, LogLevel level, LogFilter filter);
    
    public void LogTrace(LogLevel level, LogFilter filter) {
        //TODO Apply filter (If bottleneck)
        var trace = new StackTrace(1, true);
        var sb = new StringBuilder();

        sb.AppendLine("Trace: -------------");
        foreach (var frame in trace.GetFrames()) {
            sb.Append("\t");
            // var method = frame.GetMethod();
            // if (method != null) {
            //     var type = method.DeclaringType;
            //     if (type != null) {
            //         sb.Append(type.FullName + ".");
            //     }
            // }
            
            sb.Append(frame);
        }
        
        Log(sb.ToString(), level, filter);
    }
    
    public void Info(string message, LogFilter filter = LogFilter.Always) {
        Log(message, LogLevel.Info, filter);
    }
    
    public void Warning(string message, LogFilter filter = LogFilter.Always) {
        Log(message, LogLevel.Warning, filter);
    }
    
    public void Error(string message, LogFilter filter = LogFilter.Always) {
        Log(message, LogLevel.Error, filter);
    }
}