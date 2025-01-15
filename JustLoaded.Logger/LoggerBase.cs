namespace JustLoaded.Logger;

public abstract class LoggerBase : ILogger, IDisposable {
    
    protected readonly List<ILogModule> modules = new();

    protected readonly HashSet<LogFilter> filterFlags = new(new [] { LogFilter.Always });

    protected LoggerBase(params ILogModule[] modules) {
        this.modules.AddRange(modules);
    }
    
    public virtual void AddModules(params ILogModule[] modulesIn) {
        this.modules.AddRange(modulesIn.Where(module => !this.modules.Contains(module)));
    }

    public virtual void AddFilterFlags(params LogFilter[] filters) {
        filterFlags.UnionWith(filters);
    }

    public virtual void RemoveFilterFlags(params LogFilter[] filters) {
        filterFlags.ExceptWith(filters);
    }
    
    public abstract void Log(string message, LogLevel level, LogFilter filter);
    
    public virtual void Dispose() {
        foreach (var module in modules) {
            if (module is IDisposable disposable) {
                disposable.Dispose();
            }
        }
    }
}