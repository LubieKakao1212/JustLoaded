using System.Reflection;
using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Core.Loading;
using JustLoaded.Core.Reflect;

namespace JustLoaded.Loading;

public class RegisterDbReflectLoadingPhase(DBRegistrationType regType = DBRegistrationType.Main) : ILoadingPhase {

    private static readonly Type DbType = typeof(ContentDatabase<>);

    public void Load(ModLoaderSystem modLoader) {

        var masterDb = (IDatabaseRegistrationContext)modLoader.GetRequiredAttachment<IReadOnlyMasterDatabase>();
        
        foreach (var (modId, contentType) in modLoader.GetAllModTypesByAttribute<CreateDbAttribute>()) {
            var attrib = contentType.GetCustomAttribute<CreateDbAttribute>()!;
            var key = new ContentKey(modId, attrib.ContentId);
            var contentDbType = DbType.MakeGenericType(contentType);

            var db = Activator.CreateInstance(contentDbType)!;
            masterDb.RegisterDatabase(key, contentType, (IContentDatabase)db, regType);
        }
    }
}