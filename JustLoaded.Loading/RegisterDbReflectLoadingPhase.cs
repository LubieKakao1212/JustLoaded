using System.Reflection;
using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Core.Loading;
using JustLoaded.Core.Reflect;

namespace JustLoaded.Loading;

public class RegisterDbReflectLoadingPhase : ILoadingPhase {

    private static readonly Type _dbType = typeof(ContentDatabase<>);

    private DBRegistrationType _dbRegistrationType;

    public RegisterDbReflectLoadingPhase(DBRegistrationType regType = DBRegistrationType.Main) {
        _dbRegistrationType = regType;
    }
    
    public void Load(ModLoaderSystem modLoader) {

        var masterDb = (IDatabaseRegistrationContext)modLoader.MasterDb;
        
        foreach (var (modId, contentType) in modLoader.GetAllModTypesByAttribute<CreateDbAttribute>()) {
            var attrib = contentType.GetCustomAttribute<CreateDbAttribute>()!;
            var key = new ContentKey(modId, attrib.ContentId);
            var contentDbType = _dbType.MakeGenericType(contentType);

            var db = Activator.CreateInstance(contentDbType)!;
            masterDb.RegisterDatabase(key, contentType, (IContentDatabase)db, _dbRegistrationType);
        }
    }
}