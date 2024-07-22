using JustLoaded.Content.Database;
using JustLoaded.Content.Database.Extensions;

namespace JustLoaded.Content;

public struct BoundContentKey<TContent> : IEquatable<BoundContentKey<TContent>> where TContent : notnull {

    private readonly ContentKey _contentKey;
    private readonly ContentKey? _databaseKey;
    private int? _hash;

    public static BoundContentKey<TContent> Make(in ContentKey contentKey, in ContentKey? databaseKey = null) {
        return new BoundContentKey<TContent>(contentKey, databaseKey);
    }
    
    private BoundContentKey(in ContentKey contentKey, in ContentKey? databaseKey) {
        this._contentKey = contentKey;
        this._databaseKey = databaseKey;
        _hash = null;
    }

    public TContent? FetchContent(IReadOnlyMasterDatabase? masterDb = null, IReadOnlyContentDatabase? cachedDb = null) {
        return FetchContent(ref cachedDb, masterDb);
    }

    public TContent? FetchContent(ref IReadOnlyContentDatabase? cachedDb, IReadOnlyMasterDatabase? masterDb = null) {
        if (cachedDb != null) {
            if (!cachedDb.IsTypeSupported(typeof(TContent))) {
                //TODO exception
                throw new ApplicationException("Cannot fetch content, invalid database");
            }
            return cachedDb.GetContent<TContent>(_contentKey);
        }
        var master = masterDb ?? GlobalDatabases.Master;
        
        if (_databaseKey != null) {
            cachedDb = master.GetDatabase<TContent>(_databaseKey.Value);
        }
        else {
            cachedDb = master.GetByContentType<TContent>();
            cachedDb = cachedDb ?? throw new ApplicationException("No valid databes found");
        }

        return cachedDb.GetContent<TContent>(_contentKey);
    }
    
    public bool Equals(BoundContentKey<TContent> other) {
        return _contentKey.Equals(other._contentKey) && _databaseKey.Equals(other._databaseKey);
    }

    public override bool Equals(object? obj) {
        return obj is BoundContentKey<TContent> other && Equals(other);
    }

    public override int GetHashCode() {
        _hash ??= HashCode.Combine(_contentKey, _databaseKey);
        return _hash.Value;
    }
}