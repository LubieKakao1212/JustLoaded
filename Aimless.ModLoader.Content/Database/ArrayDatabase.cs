using Aimless.ModLoader.Content.Database.Execeptions;

namespace Aimless.ModLoader.Content.Database
{
    public class ArrayDatabase<TContent> : ContentDatabase<TContent> where TContent : notnull
    {
        private List<KeyValuePair<ContentKey, TContent>> orderedEntries = new();

        public override IEnumerable<TContent> ContentValues => orderedEntries.Select((entry) => entry.Value);
        public override IEnumerable<KeyValuePair<ContentKey, TContent>> ContentEntries => orderedEntries;
        public override IEnumerable<ContentKey> ContentKeys => orderedEntries.Select((entry) => entry.Key);

        public ArrayDatabase(IEnumerable<KeyValuePair<ContentKey, TContent>> orderedEntries) 
        {
            foreach (var entry in orderedEntries)
            {
                this.orderedEntries.Add(entry);
                base.AddContent(entry.Key, entry.Value);
            }
        }

        public override bool AddContent(ContentKey key, TContent value)
        {
            throw new DatabaseLockedException();
        }
    }
}
