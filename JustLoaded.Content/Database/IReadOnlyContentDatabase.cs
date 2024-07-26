namespace JustLoaded.Content.Database;

public interface IReadOnlyContentDatabase {
    /// <summary>
    /// </summary>
    /// <exception cref="UnsupportedContentTypeException">When this <see cref="IContentDatabase"/> does not support given <typeparamref name="TContent"/> content Type</exception>
    /// <returns>Requested Content</returns>
    TContent? GetContent<TContent>(ContentKey key) where TContent : notnull;

    Type[] SupportedContentTypes { get; }

    IEnumerable<ContentKey> GetContentKeys<TContent>() where TContent : notnull;
    IEnumerable<KeyValuePair<ContentKey, TContent>> GetContentEntries<TContent>() where TContent : notnull;
    IEnumerable<TContent> GetContentValues<TContent>() where TContent : notnull;
    
    bool IsTypeSupported(Type type)
    {
        return SupportedContentTypes.Contains(type);
    }
}