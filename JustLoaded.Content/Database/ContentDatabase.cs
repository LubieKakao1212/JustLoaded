using JustLoaded.Content.Database.Exceptions;

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
        
        public virtual bool AddContent(ContentKey key, TContent value)
        {
            if (IsLocked) {
                throw new DatabaseLockedException();
            }
            ContentAdded(key, ref value);
            return content.TryAdd(key, value);
        }

        public virtual TContent? GetContent(ContentKey key) {
            return content.GetValueOrDefault(key);
        }

        public virtual void Lock() {
            IsLocked = true;
            Locked();
        } 
    }
}
