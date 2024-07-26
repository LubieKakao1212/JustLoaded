
namespace JustLoaded.Content.Database.Exceptions
{
    public class DatabaseLockedException() : ApplicationException("Cannot add content to a locked ContentDatabase");
}
