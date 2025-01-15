using System.Reflection;

namespace JustLoaded.Util;

public static class ReflectHelper {
    
    public static IEnumerable<Type> GetTypesByBase<TBase>(this IEnumerable<Assembly> assemblies) {
        return GetTypesByBase(assemblies, typeof(TBase));
    }
    public static IEnumerable<Type> GetTypesByBase(this IEnumerable<Assembly> assemblies, Type baseType) {
        return assemblies.SelectMany(assembly => assembly.GetTypesByBase(baseType));
    }
    
    public static IEnumerable<Type> GetTypesByBase<TBase>(this Assembly assembly) {
        return GetTypesByBase(assembly, typeof(TBase));
    }
    public static IEnumerable<Type> GetTypesByBase(this Assembly assembly, Type baseType) {
        foreach (var type in assembly.GetTypes()) {
            if (type.IsAssignableTo(baseType)) {
                yield return type;
            }
        }
    }
    
    
    
    public static IEnumerable<Type> GetTypesByAttribute<TAttribute>(this  IEnumerable<Assembly> assemblies) where TAttribute : Attribute {
        return GetTypesByAttribute(assemblies, typeof(TAttribute));
    }
    public static IEnumerable<Type> GetTypesByAttribute(this  IEnumerable<Assembly> assemblies, Type attributeType) {
        return assemblies.SelectMany(assembly => assembly.GetTypesByAttribute(attributeType));
    }
    
    public static IEnumerable<Type> GetTypesByAttribute<TAttribute>(this Assembly assembly) where TAttribute : Attribute {
        return GetTypesByAttribute(assembly, typeof(TAttribute));
    }
    public static IEnumerable<Type> GetTypesByAttribute(this Assembly assembly, Type attributeType) {
        foreach (var type in assembly.GetTypes()) {
            if (type.GetCustomAttribute(attributeType) != null) {
                yield return type;
            }
        }
    }
    
}