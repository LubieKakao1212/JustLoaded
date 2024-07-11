using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using Aimless.ModLoader.Content;
using Aimless.ModLoader.Util;

namespace Aimless.ModLoader.Core;

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
        protected readonly Dictionary<ContentKey, Order> hardDependencies = new();
        protected readonly Dictionary<ContentKey, Order> softDependencies = new();
        
        public Builder(string modId) {
            this.modId = modId;
        }

        public Builder AddHardDependencies(Order order, params string[] deps) {
            foreach (var depId in deps) {
                hardDependencies.Add(ToModKey(depId), order);
            }
            return this;
        }
        
        public Builder AddSoftDependencies(Order order, params string[] deps) {
            foreach (var depId in deps) {
                softDependencies.Add(ToModKey(depId), order);
            }
            return this;
        }

        public ModMetadata Build() {
            return new ModMetadata() {
                ModKey = ToModKey(modId),
                HardDependencies = hardDependencies,
                SoftDependencies = softDependencies
            };
        }
    }
}