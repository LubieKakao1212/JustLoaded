namespace JustLoaded.Logger;

public class ConsoleLogModule : ILogModule {
    
    public void ProcessMessage(string message, LogLevel level, LogFilter filter) {
        Console.WriteLine(message.StampMessage(level));
    }
    
}