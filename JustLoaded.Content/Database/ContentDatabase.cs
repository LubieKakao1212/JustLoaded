using JustLoaded.Content.Database.Exceptions;
using JustLoaded.Util;

namespace JustLoaded.Content.Database
{
    public class ContentDatabase<TContent> : IContentDatabase<TContent> where TContent : notnull {
        protected readonly Dictionary<ContentKey, TContent> content = new();
        
        public event Action Locked = delegate { };
        public bool IsLocked { get; private set; }

        public event IContentDatabase.ContentAddedCallback<TContent> ContentAdded = delegate { };

        public virtual IEnumerable<TContent> ContentValues => content.Values;
        public virtual IEnumerable<KeyValuePair<ContentKey, TContent>> ContentEntries => content;
        public virtual IEnumerable<ContentKey> ContentKeys => content.Keys;
        
        public virtual TContent? GetContent(ContentKey key) {
            return content.GetValueOrDefault(key);
        }

        public virtual void Lock() {
            IsLocked = true;
            Locked();
        }

        public virtual void AddContent(ContentKey key, TContent value) {
            AddContent(key, typeof(TContent), value);
        }

        public virtual void AddContent(ContentKey key, Type type, object value) {
            if (IsLocked) {
                throw new DatabaseLockedException();
            }

            if (value is not TContent value1) {
                throw new UnsupportedContentTypeException(value.GetType(), new[] { typeof(TContent) });;
            }

            if (!content.TryAdd(key, value1)) {
                throw new ArgumentException($"Content with key {key} already exists");
            }
            
            ContentAdded(key, ref value1);
        }
 
        public bool TryAddContent(ContentKey key, TContent value) {
            return TryAddContent<TContent>(key, value);
        }

        public virtual bool TryAddContent<TContent1>(ContentKey key, TContent1 value) where TContent1 : notnull {
            if (IsLocked) {
                return false;
            }

            if (value is not TContent value1) {
                return false;
            }

            if (!content.TryAdd(key, value1)) {
                return false;
            }
            
            ContentAdded(key, ref value1);
            return true;
        }

        public virtual bool TryAddContent(ContentKey key, Type type, object value) {
            if (IsLocked) {
                return false;
            }

            if (typeof(TContent).IsAssignableFrom(type) && value is TContent value1) {
                return TryAddContent<TContent>(key, value1);
            }

            return false;
        }
    }
}
