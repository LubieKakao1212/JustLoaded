using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core.Reflect;

namespace JustLoaded.Core.Loading;

public class RegisterContentLoadingPhase : ILoadingPhase {

    private static readonly MethodInfo _genericRegister = typeof(IContentDatabase).GetMethod(nameof(IContentDatabase.AddContent))!;
    
    public void Load(ModLoaderSystem modLoader) {
        var masterDb = modLoader.MasterDb;
        var mods = (IContentDatabase<Mod>?) masterDb.GetByContentType<Mod>();
        
        //I don't like this code
        foreach (var modEntry in mods!.ContentEntries) {
            var modId = ModMetadata.ToModId(modEntry.Key);
            var mod = modEntry.Value;
            
            foreach (var assembly in mod.Assemblies) {
                foreach (var container in assembly.GetModTypeByAttribute<ContentContainerAttribute>(modId)) {
                    RuntimeHelpers.RunClassConstructor(container.TypeHandle);
                    foreach (var field in container.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)) {
                        var attrib = field.GetCustomAttribute<RegisterContentAttribute>();
                        if (attrib != null) {
                            var dbId = attrib.databaseId;
                            var contentType = field.FieldType;
                            IContentDatabase? db;
                            if (dbId == null) {
                                db = masterDb.GetByContentType(contentType);
                            }
                            else {
                                ContentKey dbKey = ToKey(dbId, modId);
                                db = masterDb.GetDatabase(dbKey, contentType);
                            }

                            if (db == null) {
                                //TODO use logger (warning)
                                Console.Error.WriteLine($"Could not find database for automatic registration for { field }");
                                continue;
                            }
                            if (!db.IsTypeSupported(contentType)) {
                                //TODO use logger (warning)
                                Console.Error.WriteLine($"Could not find database for automatic registration for { field }");
                                continue;
                            }
                            
                            var id = attrib.id;
                            id ??= FieldNameToId(field.Name);
                            var key = ToKey(id, modId);
                            var value = field.GetValue(null);
                            if (value == null) {
                                //TODO use logger (warning)
                                Console.Error.WriteLine($"Could not register a null value from field { field }");
                                continue;
                            }

                            var register = _genericRegister!.MakeGenericMethod(contentType);
                            var result = (bool) register.Invoke(db, new[] { key, value })!;
                            if (!result) {
                                //TODO use logger (warning)
                                Console.Error.WriteLine($"Could not register value from field, key { key } is already in use");
                                continue;
                            }
                            
                        }
                    }
                }
            }
        }
    }
    
    private static string FieldNameToId(string fieldName) {
        var builder = new StringBuilder(fieldName.Length);
        for(int i=0; i<fieldName.Length; i++) {
            var current = fieldName[i];
            if (current == '_') {
                current = '-';
            }
            builder.Append(char.ToLower(current));
            if (i >= fieldName.Length - 1) continue;
            if (char.IsLower(current) && char.IsUpper(fieldName[i + 1])) {
                builder.Append('-');   
            }
        }

        return builder.ToString();
    }

    private static ContentKey ToKey(string id, string modId) {
        if (id.IndexOf(':') != -1) {
            return new ContentKey(id);
        }
        return new ContentKey(modId, id);
    }
}