using JustLoaded.Content.Database.Exceptions;

namespace JustLoaded.Content.Database;

/*public class ReadOnlyDatabaseView<TContent> : IContentDatabase<TContent> where TContent : notnull {

    public IEnumerable<TContent> ContentValues => _database.ContentValues;
    public IEnumerable<KeyValuePair<ContentKey, TContent>> ContentEntries => _database.ContentEntries;
    public IEnumerable<ContentKey> ContentKeys  => _database.ContentKeys;
    
    protected readonly ContentDatabase<TContent> _database;

    public ReadOnlyDatabaseView(ContentDatabase<TContent> database) {
        this._database = database;
    }
    
    public TContent? GetContent(ContentKey key) {
        return _database.GetContent(key);
    }

    public bool AddContent(ContentKey key, TContent value) {
        throw new DatabaseLockedException();
    }
}*/