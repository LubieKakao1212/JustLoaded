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
            if (TryAddContent(key, type, value, out ContentDbAddOperationResult result)) {
                return;
            }
            
            switch (result) {
                case ContentDbAddOperationResult.Success:
                    return;
                
                case ContentDbAddOperationResult.KeyExists:
                    throw new ArgumentException($"Content with key {key} already exists");
                
                case ContentDbAddOperationResult.DatabaseLocked:
                    throw new DatabaseLockedException();
                
                case ContentDbAddOperationResult.WrongContentType:
                    throw new UnsupportedContentTypeException(value.GetType(), new[] { typeof(TContent) });;
                
                case ContentDbAddOperationResult.OtherError:
                default:
                    throw new ArgumentException($"An unknown error occurred during content addition");
            }
        }
 
        public bool TryAddContent(ContentKey key, TContent value, out ContentDbAddOperationResult result) {
            return TryAddContent<TContent>(key, value, out result);
        }

        public virtual bool TryAddContent<TContent1>(ContentKey key, TContent1 value, out ContentDbAddOperationResult result) where TContent1 : notnull {
            if (IsLocked) {
                result = ContentDbAddOperationResult.DatabaseLocked;
                return false;
            }

            if (value is not TContent value1) {
                result = ContentDbAddOperationResult.WrongContentType;
                return false;
            }

            if (!content.TryAdd(key, value1)) {
                result = ContentDbAddOperationResult.KeyExists;
                return false;
            }
            
            ContentAdded(key, ref value1);
            result = ContentDbAddOperationResult.Success;
            return true;
        }

        public virtual bool TryAddContent(ContentKey key, Type type, object value, out ContentDbAddOperationResult result) {
            if (IsLocked) {
                result = ContentDbAddOperationResult.DatabaseLocked;
                return false;
            }

            if (typeof(TContent).IsAssignableFrom(type) && value is TContent value1) {
                return TryAddContent<TContent>(key, value1, out result);
            }

            result = ContentDbAddOperationResult.WrongContentType;
            return false;
        }
    }
}
