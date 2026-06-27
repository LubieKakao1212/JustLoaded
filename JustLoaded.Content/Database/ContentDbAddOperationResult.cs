namespace JustLoaded.Content.Database;

public enum ContentDbAddOperationResult
{
    Success = 0,
    OtherError = 1,
    KeyExists = 2,
    DatabaseLocked = 3,
    WrongContentType = 4,
}