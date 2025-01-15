using JustLoaded.Content.Database.Exceptions;

namespace JustLoaded.Content.Database;

/// <summary>
/// MasterDatabase contains <see cref="IContentDatabase"/> instances <br/>
/// Contains a reference to itself under key "core:database"
/// </summary>
public class MasterDatabase : ContentDatabase<IContentDatabase>, IReadOnlyMasterDatabase, IDatabaseRegistrationContext
{
    private readonly Dictionary<Type, ContentKey> _keysByType = new();

    public MasterDatabase() {
        AddContent(new ContentKey("core", "database"), typeof(IContentDatabase), this);
    }
    
    public ContentKey? KeyByContentType(Type type)
    {
        if (_keysByType.TryGetValue(type, out var key))
        {
            return key;
        }
        return null;
    }

    public IContentDatabase? GetByContentType(Type type)
    {
        if (!_keysByType.TryGetValue(type, out var key))
        {
            return null;
        }
        return GetContent(key);
    }

    private bool AddContent(ContentKey key, Type type, IContentDatabase value)
    {
        if (_keysByType.ContainsKey(type))
        {
            return false;
        }
        if (!base.AddContent(key, value))
        {
            return false;
        }
        _keysByType.Add(type, key);
        return true;
    }

    /// <summary>
    /// Creates a new <see cref="ContentDatabase{TContent}"/> and registers it to this <see cref="MasterDatabase"/>"/>
    /// </summary>
    /// <param name="key">The <see cref="ContentKey"/> associated with the created database</param>
    /// <param name="registrationType"></param>
    /// <returns></returns>
    public IContentDatabase<TContent> CreateDatabase<TContent>(ContentKey key, DBRegistrationType registrationType = DBRegistrationType.Any) where TContent : notnull 
    {
        var db = new ContentDatabase<TContent>();
        RegisterDatabase(key, typeof(TContent), db, registrationType);
        return db;
    }

    public void RegisterDatabase<TContent>(ContentKey key, ContentDatabase<TContent> database, DBRegistrationType registrationType = DBRegistrationType.Any) where TContent : notnull {
        RegisterDatabase(key, typeof(TContent), database, registrationType);
    }
    
    public void RegisterDatabase<TContent>(ContentKey key, IContentDatabase database, DBRegistrationType registrationType = DBRegistrationType.Any) {
        RegisterDatabase(key, typeof(TContent), database, registrationType);
    }
    
    public void RegisterDatabase(ContentKey key, Type? type, IContentDatabase database, DBRegistrationType registrationType = DBRegistrationType.Any)
    {
        if (type != null)
        {
            AssertContentType(database, type);
        }

        bool flag = false;
        switch (registrationType)
        {
            case DBRegistrationType.Any:
                if (type != null)
                {
                    flag = this.AddContent(key, type, database);
                }
                goto case DBRegistrationType.Secondary;
            case DBRegistrationType.Main:
                if (type == null)
                {
                    throw new ContentRegistrationException($"Failed to register Main database [{key}] for type null");
                }
                if (!this.AddContent(key, type, database))
                {
                    throw new ContentRegistrationException($"Failed to register Main database [{key}] for type: {type}, key or type already set");
                }
                return;
            case DBRegistrationType.Secondary:
                if (!flag)
                {
                    this.AddContent(key, database);
                }
                break;
        }
    }
    
    /*//TODO exception type
    /// <exception cref="ApplicationException">when no database is found</exception>
    /// <exception cref="UnsupportedContentTypeException">when database under <paramref name="key"/> does not support <typeparamref name="TContent"/></exception>
    public IContentDatabase GetDatabase<TContent>(ContentKey? key)
    {
        var contentType = typeof(TContent);
        IContentDatabase? db;
        if (key != null)
        {
            db = this.GetContent(key.Value);
        }
        else
        {
            db = this.GetByContentType(contentType);
        }
        if (db == null)
        {
            throw new ApplicationException();
        }
        AssertContentType(db, contentType);
        return db;
    }*/
    
    public IContentDatabase GetDatabase(ContentKey? key, Type type) {
        if (key == null) {
            key = KeyByContentType(type);
        }

        if (key == null) {
            //TODO exception
            throw new ApplicationException($"No registered database for content type { type }");
        }
        
        var db = GetContent(key.Value);
        
        //TODO exception
        db = db ?? throw new ApplicationException($"Database with key { key } could not be found");

        if (!db.IsTypeSupported(type)) {
            throw new ArgumentException($"Database with key { key } does not support requested Type { type }");
        }

        return db;
    }
    
    private static void AssertContentType(IContentDatabase db, Type contentType)
    {
        if (!db.IsTypeSupported(contentType)) 
        { 
            throw new UnsupportedContentTypeException(contentType, db.SupportedContentTypes);
        }
    }
}
