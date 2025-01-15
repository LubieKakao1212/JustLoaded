using System.Collections.Concurrent;

namespace JustLoaded.Logger;

/// <summary>
/// Modules used by an AsyncLogger may not be used by another ILogger
/// </summary>
public class AsyncLogger : LoggerBase {
    
    private Thread? _thread;

    private readonly ConcurrentQueue<(string message, LogLevel level, LogFilter filter)> _queue = new();

    private readonly Semaphore _semaphore = new Semaphore(0, int.MaxValue);

    private readonly TimeSpan _timeout;
    
    /// <summary>
    /// Modules used by an AsyncLogger may not be used by another ILogger
    /// </summary>
    public AsyncLogger(TimeSpan semaphoreTimeout, params ILogModule[] modules) : base(modules) {
        _timeout = semaphoreTimeout;
    }

    public override void Log(string message, LogLevel level, LogFilter filter) {
        if (!filterFlags.Contains(filter)) {
            return;
        }
        _queue.Enqueue((message, level, filter));
        _semaphore.Release();
    }

    public override void AddModules(params ILogModule[] modulesIn) {
        lock (base.modules) {
            base.AddModules(modulesIn);   
        }
    }
    
    public void Start() {
        if (_thread != null) {
            throw new ApplicationException("Logger thread already started");
        }
        
        _thread = new Thread(ThreadLoop);
        _thread.Start();
    }

    private void ThreadLoop() {
        do {
            _semaphore.WaitOne(_timeout);
            _queue.TryDequeue(out var message);
            PrintMessage(message);
        } while (_thread != null);
        
        //Dispose was called
        //1. Print remaining messages
        while (_queue.TryDequeue(out var message)) {
            PrintMessage(message);
        }
        
        //2. Dispose modules
        lock (modules) {
            base.Dispose();
        }
    }

    private void PrintMessage((string message, LogLevel level, LogFilter filter) message) {
        lock (modules) {
            foreach (var module in modules) {
                module.ProcessMessage(message.message, message.level, message.filter);
            }
        }
    }
    
    public override void Dispose() {
        _thread = null;
    }
}