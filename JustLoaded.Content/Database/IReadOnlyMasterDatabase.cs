namespace JustLoaded.Content.Database;

public interface IReadOnlyMasterDatabase : IReadOnlyContentDatabase {
    public ContentKey? KeyByContentType<TContent>()
    {
        return KeyByContentType(typeof(TContent));
    }

    public IContentDatabase? GetByContentType<TContent>()
    {
        return GetByContentType(typeof(TContent));
    }

    public ContentKey? KeyByContentType(Type type);

    public IContentDatabase? GetByContentType(Type type);
    
    public IContentDatabase GetDatabase<TContent>() where TContent : notnull
    {
        return GetDatabase<TContent>(null);
    }
    
    /// <returns>Acquired <see cref="IContentDatabase"/> is guaranteed to support <typeparamref name="TContent"/> and is guaranteed to not be null</returns>
    /// <exception cref="ApplicationException">If there is no database under specified <paramref name="key"/> or database under <paramref name="key"/> does not support <typeparamref name="TContent"/></exception> //TODO Excetopn type
    public IContentDatabase GetDatabase<TContent>(ContentKey? key) where TContent : notnull {
        return GetDatabase(key, typeof(TContent));
    }
    
    public IContentDatabase GetDatabase(ContentKey? key, Type contentType);
}