namespace JustLoaded.Util.Attachment;

public interface IMutableAttachmentProvider<out TSelf> : IAttachmentProvider {
    
    TSelf AddAttachment<T>(T attachment) where T : class;
    
    T GetOrAddAttachment<T>(Func<T> constructor) where T : class;
    
}