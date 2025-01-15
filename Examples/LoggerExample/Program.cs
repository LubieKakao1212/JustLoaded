// See https://aka.ms/new-console-template for more information

using JustLoaded.Logger;
using LoggerExample;

using AsyncLogger loggerAsync = new AsyncLogger(TimeSpan.FromSeconds(2),
    new ConsoleLogModule(),
    new StreamLogModule(File.Open("log.log", FileMode.Create, FileAccess.Write, FileShare.Read))
    );
loggerAsync.AddFilterFlags(LogFilter.Debug);

loggerAsync.Start();

var logger = (ILogger)loggerAsync;

void StackFrame() {
    logger.LogTrace(LogLevel.Info, LogFilter.Debug);
    SomeClass.SomeMethod(logger);
}

logger.Info("This is an info");
logger.Warning("This is a warning");
logger.Error("This is an error");

logger.Info("This is a debug info", LogFilter.Debug);
logger.Warning("This is a debug warning", LogFilter.Debug);
logger.Error("This is a debug error", LogFilter.Debug);

logger.LogTrace(LogLevel.Info, LogFilter.Debug);

StackFrame();

for (int i = 0; i < 1000000; i++) {
    logger.Info("Value of \"i\"" + i);
}

File.Create(Random.Shared.Next()+".fin");

Console.WriteLine("Hello From the end");