namespace JustLoaded.Util.Attachment;

public class MissingAttachmentException(Type attachmentType) : Exception($"No attachment of type {attachmentType}") { }