namespace JustLoaded.Logger;

public interface ILogModule {
    public void ProcessMessage(string message, LogLevel level, LogFilter filter);
}