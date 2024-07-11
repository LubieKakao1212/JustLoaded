using System.Reflection;

namespace Aimless.ModLoader.Core.Reflect;

public static class ReflectionUtil {

    public static IEnumerable<Type> GetModTypeByBase<TBase>(this Assembly assembly, string modId, bool includeNotAnnotated = false) {
        return GetModTypeByBase(assembly, modId, typeof(TBase), includeNotAnnotated);
    }
    
    public static IEnumerable<Type> GetModTypeByBase(this Assembly assembly, string modId, Type baseType, bool includeNotAnnotated = false) {
        foreach (var type in assembly.GetTypeByBase(baseType)) {
            if (type.IsFromMod(modId, includeNotAnnotated)) {
                yield return type;
            }
        }
    }
    
    public static IEnumerable<Type> GetTypeByBase<TBase>(this Assembly assembly) {
        return GetTypeByBase(assembly, typeof(TBase));
    }
    
    public static IEnumerable<Type> GetTypeByBase(this Assembly assembly, Type baseType) {
        foreach (var type in assembly.GetTypes()) {
            if (type.IsAssignableTo(baseType)) {
                yield return type;
            }
        }
    }

    public static IEnumerable<Type> GetModTypeByAttribute<TAttribute>(this Assembly assembly, string modId, bool includeNotAnnotated = false) where TAttribute : Attribute {
        return GetModTypeByAttribute(assembly, modId, typeof(TAttribute), includeNotAnnotated);
    }
    
    public static IEnumerable<Type> GetModTypeByAttribute(this Assembly assembly, string modId, Type attributeType, bool includeNotAnnotated = false) {
        foreach (var type in assembly.GetTypeByAttribute(attributeType)) {
            if (type.IsFromMod(modId, includeNotAnnotated)) {
                yield return type;
            }
        }
    }
    
    public static IEnumerable<Type> GetTypeByAttribute<TAttribute>(this Assembly assembly) where TAttribute : Attribute {
        return GetTypeByAttribute(assembly, typeof(TAttribute));
    }
    
    public static IEnumerable<Type> GetTypeByAttribute(this Assembly assembly, Type attributeType) {
        foreach (var type in assembly.GetTypes()) {
            if (type.GetCustomAttribute(attributeType) != null) {
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