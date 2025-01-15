namespace JustLoaded.Logger;

public class StreamLogModule : ILogModule, IDisposable {

    private readonly StreamWriter _file;

    public StreamLogModule(FileStream stream) {
        _file = new(stream);
    }

    public void ProcessMessage(string message, LogLevel level, LogFilter filter) {
        _file.WriteLine(message.StampMessage(level));
        _file.Flush();
    }
    
    public void Dispose() {
        _file.Dispose();
    }
}