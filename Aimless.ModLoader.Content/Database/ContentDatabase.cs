
using System.Collections;

namespace Aimless.ModLoader.Content.Database
{
    public class ContentDatabase<T> : IContentDatabase<T> where T : notnull
    {
        private readonly Dictionary<string, T> content = new();

        public IEnumerable<T> ContentValues => content.Values;
        public IEnumerable<KeyValuePair<string, T>> ContentEntries => content;
        public IEnumerable<string> ContentKeys => content.Keys;

        public ContentDatabase()
        {

        }
        
        public virtual bool AddContent(string key, T value)
        {
            return content.TryAdd(key, value);
        }

        public virtual T? GetContent(string key)
        {
            if (!content.TryGetValue(key, out var val))
            {
                return default;
            }
            return val;
        }
    }
}
