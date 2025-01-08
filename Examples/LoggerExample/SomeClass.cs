using JustLoaded.Logger;

namespace LoggerExample;

public class SomeClass {

    public static void SomeMethod(ILogger logger) {
        logger.LogTrace(LogLevel.Info, LogFilter.Debug);
        new SomeClass().SomeInstanceMethod(logger);
    }
    
    public void SomeInstanceMethod(ILogger logger) {
        logger.Info("nbdi ulfgiAB Vgyu bhscy g jhsgyuxh");
        logger.Info("nbdi ulfgiAB Vgyu bhscy g jhsgyuxh");
        logger.Info("nbdi ulfgiAB Vgyu bhscy g jhsgyuxh");
        logger.Info("nbdi ulfgiAB Vgyu bhscy g jhsgyuxh");
        logger.Info("nbdi ulfgiAB Vgyu bhscy g jhsgyuxh");
        logger.Info("nbdi ulfgiAB Vgyu bhscy g jhsgyuxh");
        logger.LogTrace(LogLevel.Info, LogFilter.Debug);
    }
}