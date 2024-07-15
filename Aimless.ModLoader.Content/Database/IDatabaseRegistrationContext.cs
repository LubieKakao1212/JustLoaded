namespace Aimless.ModLoader.Content.Database;

public interface IDatabaseRegistrationContext {

    /// <summary>
    /// Creates and registers a new <see cref="IContentDatabase{TContent}"/>
    /// </summary>
    /// <param name="key">The <see cref="ContentKey"/> associated with the created database</param>
    /// <param name="registrationType"></param>
    /// <returns>The created <see cref="IContentDatabase{TContent}"/></returns>
    public IContentDatabase<TContent> CreateDatabase<TContent>(ContentKey key,
        DBRegistrationType registrationType = DBRegistrationType.Any) where TContent : notnull;

    public void RegisterDatabase<TContent>(ContentKey key, ContentDatabase<TContent> database,
        DBRegistrationType registrationType = DBRegistrationType.Any) where TContent : notnull;

    public void RegisterDatabase<TContent>(ContentKey key, IContentDatabase database,
        DBRegistrationType registrationType = DBRegistrationType.Any);

}