using System.Reflection;
using JustLoaded.Util;

namespace JustLoaded.Core.Reflect;

public static class ReflectionUtil {

    #region GetAllTypes
    #region ByBase
    public static IEnumerable<(string modId, Type type)> GetAllModTypesByBase<TBase>(this ModLoaderSystem modLoader) {
        return modLoader.GetAllModTypesByBase(typeof(TBase));
    }
    public static IEnumerable<(string modId, Type type)> GetAllModTypesByBase(this ModLoaderSystem modLoader, Type baseType) {
        return modLoader.GetAllModTypesByExtractor(assembly => assembly.GetTypesByBase(baseType));
    }
    #endregion

    #region ByAttribute
    public static IEnumerable<(string modId, Type type)> GetAllModTypesByAttribute<TAttribute>(this ModLoaderSystem modLoader) where TAttribute : Attribute {
        return modLoader.GetAllModTypesByAttribute(typeof(TAttribute));
    }
    public static IEnumerable<(string modId, Type type)> GetAllModTypesByAttribute(this ModLoaderSystem modLoader, Type attributeType) {
        return modLoader.GetAllModTypesByExtractor(assembly => assembly.GetTypesByAttribute(attributeType));
    }
    #endregion
    
    public static IEnumerable<(string, Type)> GetAllModTypesByExtractor(this ModLoaderSystem modLoader, Func<Assembly, IEnumerable<Type>> typeExtractor) {
        var mods = modLoader.MasterDb.GetDatabase<Mod>().GetContentEntries<Mod>();

        foreach (var mod in mods) {
            foreach (var assembly in mod.Value.Assemblies) {
                foreach (var type in typeExtractor(assembly)) {
                    yield return (mod.Key.path, type);
                }
            }
        }
    }
    
    #endregion
    
    public static IEnumerable<Type> GetModTypeByBase<TBase>(this Assembly assembly, string modId, bool includeNotAnnotated = false) {
        return GetModTypeByBase(assembly, modId, typeof(TBase), includeNotAnnotated);
    }
    
    public static IEnumerable<Type> GetModTypeByBase(this Assembly assembly, string modId, Type baseType, bool includeNotAnnotated = false) {
        foreach (var type in assembly.GetTypesByBase(baseType)) {
            if (type.IsFromMod(modId, includeNotAnnotated)) {
                yield return type;
            }
        }
    }
    
    public static IEnumerable<Type> GetModTypeByAttribute<TAttribute>(this Assembly assembly, string modId, bool includeNotAnnotated = false) where TAttribute : Attribute {
        return GetModTypeByAttribute(assembly, modId, typeof(TAttribute), includeNotAnnotated);
    }
    
    public static IEnumerable<Type> GetModTypeByAttribute(this Assembly assembly, string modId, Type attributeType, bool includeNotAnnotated = false) {
        foreach (var type in assembly.GetTypesByAttribute(attributeType)) {
            if (type.IsFromMod(modId, includeNotAnnotated)) {
                yield return type;
            }
        }
    }
    
    public static bool IsFromMod(this Type type, string modId, bool includeNotAnnotated = false) {
        var modAttrib = type.GetCustomAttribute<FromModAttribute>();

        if (modAttrib != null) {
            if (modAttrib.modId == modId) {
                return true;
            }
        }
        else if (includeNotAnnotated) {
            return true;
        }

        return false;
    }
    
}