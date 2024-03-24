using System.Diagnostics.CodeAnalysis;

namespace Aimless.ModLoader.Content.Database
{
    public interface IContentDatabase
    {
        /// <summary>
        /// </summary>
        /// <exception cref="UnsupportedContentTypeException">When this <see cref="IContentDatabase"/> does not support given <typeparamref name="T"/> content Type</exception>
        /// <returns>True if element was succesfully added, False if given key already exists</returns>
        bool AddContent<TContent>(string key, TContent value) where TContent : notnull;


        /// <summary>
        /// </summary>
        /// <exception cref="UnsupportedContentTypeException">When this <see cref="IContentDatabase"/> does not support given <typeparamref name="T"/> content Type</exception>
        /// <returns>Requested Content</returns>
        [return: MaybeNull]
        TContent GetContent<TContent>(string key) where TContent : notnull;
    }

    public interface IContentDatabase<TContent> : IContentDatabase where TContent : notnull
    {
        IEnumerable<TContent> ContentValues { get; }
        IEnumerable<KeyValuePair<string, TContent>> ContentEntries { get; }
        IEnumerable<string> ContentKeys { get; }

        TContent? GetContent(string key);

        bool AddContent(string key, TContent value);

        [return: MaybeNull]
        TContent1 IContentDatabase.GetContent<TContent1>(string key)
        {
            if (typeof(TContent1) != typeof(TContent))
            {
                throw new UnsupportedContentTypeException(typeof(TContent1), new Type[] { typeof(TContent) });
            }

            var value = GetContent(key);

            if (value is TContent1 valueT1)
            {
                return valueT1;
            }
            return default;
        }
        
        bool IContentDatabase.AddContent<TContent1>(string key, TContent1 value)
        {
            if (typeof(TContent1) != typeof(TContent))
            {
                throw new UnsupportedContentTypeException(typeof(TContent1), new Type[] { typeof(TContent) });
            }

            if (value is TContent valueT)
            {
                return AddContent(key, valueT);
            }
            throw new UnsupportedContentTypeException(typeof(TContent1), new Type[] { typeof(TContent) });
        }
    }
}
