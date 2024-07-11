namespace Aimless.ModLoader.Content.Database;

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

    public IContentDatabase? GetContent(ContentKey key);
}