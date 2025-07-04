using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Core.Loading;
using JustLoaded.Core.Reflect;
using JustLoaded.Logger;

namespace JustLoaded.Loading;

public class RegisterContentLoadingPhase : ILoadingPhase {
    
    public void Load(ModLoaderSystem modLoader) {
        var masterDb = modLoader.MasterDb;
        var mods = (IContentDatabase<Mod>?) masterDb.GetByContentType<Mod>();

        var logger = modLoader.GetAttachment<ILogger>();
        
        //I don't like this code
        foreach (var modEntry in mods!.ContentEntries.Reverse() /*Reversing the order so mods which are last have most priority in filling the databases*/) {
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
                                logger?.Error($"Could not find database for automatic registration for { field }");
                                continue;
                            }
                            if (!db.IsTypeSupported(contentType)) {
                                logger?.Error($"Could not find database for automatic registration for { field }");
                                continue;
                            }
                            
                            var id = attrib.id;
                            id ??= FieldNameToId(field.Name);
                            var key = ToKey(id, modId);
                            var value = field.GetValue(null);
                            if (value == null) {
                                logger?.Error($"Could not register a null value from field { field }");
                                continue;
                            }
                            
                            var result = db.AddContent(key, value, contentType);
                            if (!result) {
                                logger?.Error($"Could not register value from field, key { key } is already in use");
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