using System.Diagnostics.CodeAnalysis;
using JustLoaded.Util;
using JustLoaded.Util.Attachment;

namespace JustLoaded.Core;

public class ModMetadata : IAttachmentProvider {

    public string ModId { get; protected init; }
    
    [NotNull] protected internal Dictionary<string, Order>? HardDependencies { get; protected init; }
    [NotNull] protected internal Dictionary<string, Order>? SoftDependencies { get; protected init; }
    
    private readonly IAttachmentProvider _attachmentProviderImpl = new AttachmentProviderBase();

    private ModMetadata() {
        
    }

    public IEnumerable<string> ListDependencies(ModDependencyType depType, Order depOrder) {
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
    
    public static Builder Create(string modId) {
        return new Builder(modId);
    }

    #region AttachmentProvider

    public T? GetAttachment<T>() where T : class {
        return _attachmentProviderImpl.GetAttachment<T>();
    }

    public T GetRequiredAttachment<T>() where T : class {
        return _attachmentProviderImpl.GetRequiredAttachment<T>();
    }

    public bool HasAttachment<T>() where T : class {
        return _attachmentProviderImpl.HasAttachment<T>();
    }

    #endregion
    
    
    public class Builder : IMutableAttachmentProvider<Builder> {

        protected readonly string modId;
        protected readonly Dictionary<string, Order> requiredDependencies = new();
        protected readonly Dictionary<string, Order> optionalDependencies = new();

        private readonly IMutableAttachmentProvider<AttachmentProviderImpl> _attachmentProviderImpl = new AttachmentProviderImpl();

        public Builder(string modId) {
            this.modId = modId;
        }

        public Builder AddRequiredDependencies(Order order, params string[] deps) {
            foreach (var depId in deps) {
                requiredDependencies.Add(depId, order);
            }
            return this;
        }
        
        public Builder AddOptionalDependencies(Order order, params string[] deps) {
            foreach (var depId in deps) {
                optionalDependencies.Add(depId, order);
            }
            return this;
        }

        public Builder AddRequiredDependency(Order order, string depId) {
            requiredDependencies.Add(depId, order);
            return this;
        }
        
        public Builder AddOptionalDependency(Order order, string depId) {
            optionalDependencies.Add(depId, order);
            return this;
        }
        
        public ModMetadata Build() {
            return new ModMetadata() {
                ModId = modId,
                HardDependencies = requiredDependencies,
                SoftDependencies = optionalDependencies
            };
        }

        #region AttachmentProvider
        public T? GetAttachment<T>() where T : class {
            return _attachmentProviderImpl.GetAttachment<T>();
        }

        public T GetRequiredAttachment<T>() where T : class {
            return _attachmentProviderImpl.GetRequiredAttachment<T>();
        }

        public bool HasAttachment<T>() where T : class {
            return _attachmentProviderImpl.HasAttachment<T>();
        }

        public Builder AddAttachment<T>(T attachment) where T : class {
            _attachmentProviderImpl.AddAttachment(attachment);
            return this;
        }
        
        public T GetOrAddAttachment<T>(Func<T> constructor) where T : class {
            return _attachmentProviderImpl.GetOrAddAttachment(constructor);
        }
        #endregion
    }
}