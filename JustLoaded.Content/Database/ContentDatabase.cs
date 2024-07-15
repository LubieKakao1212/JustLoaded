namespace JustLoaded.Content.Database
{
    public class ContentDatabase<TContent> : IContentDatabase<TContent> where TContent : notnull
    {
        protected readonly Dictionary<ContentKey, TContent> content = new();

        public virtual IEnumerable<TContent> ContentValues => content.Values;
        public virtual IEnumerable<KeyValuePair<ContentKey, TContent>> ContentEntries => content;
        public virtual IEnumerable<ContentKey> ContentKeys => content.Keys;
        
        public virtual bool AddContent(ContentKey key, TContent value)
        {
            return content.TryAdd(key, value);
        }

        public virtual TContent? GetContent(ContentKey key) {
            return content.GetValueOrDefault(key);
        }
    }
}
