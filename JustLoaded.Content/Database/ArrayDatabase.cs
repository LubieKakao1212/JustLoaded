using JustLoaded.Content.Database.Exceptions;

namespace JustLoaded.Content.Database
{
    public class ArrayDatabase<TContent> : ContentDatabase<TContent> where TContent : notnull
    {
        private List<KeyValuePair<ContentKey, TContent>> orderedEntries = new();

        public override IEnumerable<TContent> ContentValues => orderedEntries.Select((entry) => entry.Value);
        public override IEnumerable<KeyValuePair<ContentKey, TContent>> ContentEntries => orderedEntries;
        public override IEnumerable<ContentKey> ContentKeys => orderedEntries.Select((entry) => entry.Key);
        
        public ArrayDatabase() 
        {
            
        }

        public void Init(IEnumerable<KeyValuePair<ContentKey, TContent>> orderedEntries) {
            if (IsLocked) {
                throw new DatabaseLockedException();
            }
            
            foreach (var entry in orderedEntries)
            {
                this.orderedEntries.Add(entry);
                base.AddContent(entry.Key, entry.Value);
            }
            
            Lock();
        }
        
        public override bool AddContent(ContentKey key, TContent value)
        {
            throw new DatabaseLockedException();
        }
    }
}
