using System.Diagnostics.CodeAnalysis;

namespace Aimless.ModLoader.Content.Database
{
    public interface IContentDatabase
    {
        /// <summary>
        /// </summary>
        /// <exception cref="UnsupportedContentTypeException">When this <see cref="IContentDatabase"/> does not support given <typeparamref name="T"/> content Type</exception>
        /// <returns>True if element was succesfully added, False if given key already exists</returns>
        bool AddContent<TContent>(ContentKey key, TContent value) where TContent : notnull;


        /// <summary>
        /// </summary>
        /// <exception cref="UnsupportedContentTypeException">When this <see cref="IContentDatabase"/> does not support given <typeparamref name="T"/> content Type</exception>
        /// <returns>Requested Content</returns>
        [return: MaybeNull]
        TContent GetContent<TContent>(ContentKey key) where TContent : notnull;

        Type[] SupportedContentTypes { get; }

        bool IsTypeSuported(Type type)
        {
            return SupportedContentTypes.Contains(type);
        }
    }

    public interface IContentDatabase<TContent> : IContentDatabase where TContent : notnull
    {
        IEnumerable<TContent> ContentValues { get; }
        IEnumerable<KeyValuePair<ContentKey, TContent>> ContentEntries { get; }
        IEnumerable<ContentKey> ContentKeys { get; }

        Type[] IContentDatabase.SupportedContentTypes => new Type[] { typeof(TContent) };

        TContent? GetContent(ContentKey key);

        bool AddContent(ContentKey key, TContent value);

        [return: MaybeNull]
        TContent1 IContentDatabase.GetContent<TContent1>(ContentKey key)
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
        
        bool IContentDatabase.AddContent<TContent1>(ContentKey key, TContent1 value)
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
