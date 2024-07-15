using System.Diagnostics.CodeAnalysis;
using JustLoaded.Content.Database.Execeptions;

namespace JustLoaded.Content.Database
{
    public interface IContentDatabase : IReadOnlyContentDatabase
    {
        /// <summary>
        /// </summary>
        /// <exception cref="UnsupportedContentTypeException">When this <see cref="IContentDatabase"/> does not support given <typeparamref name="TContent"/> content Type</exception>
        /// <returns>True if element was successfully added, False if given key already exists</returns>
        bool AddContent<TContent>(ContentKey key, TContent value) where TContent : notnull;
    }

    public interface IContentDatabase<TContent> : IContentDatabase where TContent : notnull
    {
        IEnumerable<TContent> ContentValues { get; }
        IEnumerable<KeyValuePair<ContentKey, TContent>> ContentEntries { get; }
        IEnumerable<ContentKey> ContentKeys { get; }

        Type[] IReadOnlyContentDatabase.SupportedContentTypes => new[] { typeof(TContent) };

        TContent? GetContent(ContentKey key);

        bool AddContent(ContentKey key, TContent value);

        [return: MaybeNull]
        TContent1 IReadOnlyContentDatabase.GetContent<TContent1>(ContentKey key)
        {
            if (typeof(TContent1) != typeof(TContent))
            {
                throw new UnsupportedContentTypeException(typeof(TContent1), new[] { typeof(TContent) });
            }

            var value = GetContent(key);

            if (value is TContent1 valueT1)
            {
                return valueT1;
            }
            return default;
        }
        
        bool IContentDatabase.AddContent<TContent1>(ContentKey key, TContent1 value)
        {
            if (typeof(TContent1) != typeof(TContent))
            {
                throw new UnsupportedContentTypeException(typeof(TContent1), new[] { typeof(TContent) });
            }

            if (value is TContent valueT)
            {
                return AddContent(key, valueT);
            }
            throw new UnsupportedContentTypeException(typeof(TContent1), new[] { typeof(TContent) });
        }
    }
}
