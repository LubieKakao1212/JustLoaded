using System.Diagnostics.CodeAnalysis;
using JustLoaded.Content;
using JustLoaded.Util;

namespace JustLoaded.Core;

public class ModMetadata {

    public ContentKey ModKey { get; protected init; }
    
    [NotNull] protected internal Dictionary<ContentKey, Order>? HardDependencies { get; protected init; }
    [NotNull] protected internal Dictionary<ContentKey, Order>? SoftDependencies { get; protected init; }
    
    protected ModMetadata() {
        
    }

    public IEnumerable<ContentKey> ListDependencies(ModDependencyType depType, Order depOrder) {
        var deps = depType switch {
            ModDependencyType.Required => HardDependencies,
            ModDependencyType.Optional => SoftDependencies,
            _ => throw new NotImplementedException()
        };

        foreach (var dep in deps) {
            if (dep.Value == depOrder) {
                yield return dep.Key;
            }
        }
    }

    public static ContentKey ToModKey(string modId) {
        return new ContentKey("mod", modId);
    }
    
    public static string ToModId(in ContentKey modKey) {
        return modKey.path;
    }

    public static Builder Create(string modId) {
        return new Builder(modId);
    }
    
    public class Builder {

        protected readonly string modId;
        protected readonly Dictionary<ContentKey, Order> requiredDependencies = new();
        protected readonly Dictionary<ContentKey, Order> optionalDependencies = new();
        
        public Builder(string modId) {
            this.modId = modId;
        }

        public Builder AddRequiredDependencies(Order order, params string[] deps) {
            foreach (var depId in deps) {
                requiredDependencies.Add(ToModKey(depId), order);
            }
            return this;
        }
        
        public Builder AddOptionalDependencies(Order order, params string[] deps) {
            foreach (var depId in deps) {
                optionalDependencies.Add(ToModKey(depId), order);
            }
            return this;
        }

        public Builder AddRequiredDependency(Order order, string depId) {
            requiredDependencies.Add(ToModKey(depId), order);
            return this;
        }
        
        public Builder AddOptionalDependency(Order order, string depId) {
            optionalDependencies.Add(ToModKey(depId), order);
            return this;
        }


        
        public ModMetadata Build() {
            return new ModMetadata() {
                ModKey = ToModKey(modId),
                HardDependencies = requiredDependencies,
                SoftDependencies = optionalDependencies
            };
        }
    }
}