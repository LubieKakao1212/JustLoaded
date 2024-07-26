using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using JustLoaded.Content.Database.Exceptions;

namespace JustLoaded.Content.Database
{
    public interface IContentDatabase : IReadOnlyContentDatabase {
        
        delegate void ContentAddedCallback<TContent>(ContentKey key, ref TContent content) where TContent : notnull;
        event Action Locked;
        
        bool IsLocked { get; }

        /// <summary>
        /// </summary>
        /// <exception cref="UnsupportedContentTypeException">When this <see cref="IContentDatabase"/> does not support given <typeparamref name="TContent"/> content Type</exception>
        /// <returns>True if element was successfully added, False if given key already exists</returns>
        /// <exception cref="DatabaseLockedException">When this database is locked (<see cref="IsLocked"/>)</exception>
        bool AddContent<TContent>(ContentKey key, TContent value) where TContent : notnull;
        
        void RegisterContentAddedCallback<TContent>(ContentAddedCallback<TContent> callback) where TContent : notnull;
    }

    public interface IContentDatabase<TContent> : IContentDatabase where TContent : notnull {
        
        event ContentAddedCallback<TContent> ContentAdded;
        
        IEnumerable<TContent> ContentValues { get; }
        IEnumerable<KeyValuePair<ContentKey, TContent>> ContentEntries { get; }
        IEnumerable<ContentKey> ContentKeys { get; }

        Type[] IReadOnlyContentDatabase.SupportedContentTypes => new[] { typeof(TContent) };

        void IContentDatabase.RegisterContentAddedCallback<TContent1>(ContentAddedCallback<TContent1> callback) {
            AssertType<TContent1>();
            ContentAdded += callback as ContentAddedCallback<TContent>;
        }

        IEnumerable<ContentKey> IReadOnlyContentDatabase.GetContentKeys<TContent1>() {
            AssertType<TContent1>();
            return ContentKeys;
        }

        IEnumerable<KeyValuePair<ContentKey, TContent1>> IReadOnlyContentDatabase.GetContentEntries<TContent1>() {
            AssertType<TContent1>();
            //Should work?
            return (IEnumerable<KeyValuePair<ContentKey, TContent1>>)ContentEntries;
        }

        IEnumerable<TContent1> IReadOnlyContentDatabase.GetContentValues<TContent1>() {
            AssertType<TContent1>();
            //Should work?
            return (IEnumerable<TContent1>)ContentValues;
        }

        TContent? GetContent(ContentKey key);
        
        bool AddContent(ContentKey key, TContent value);

        [return: MaybeNull]
        TContent1 IReadOnlyContentDatabase.GetContent<TContent1>(ContentKey key)
        {
            AssertType<TContent1>();
            var value = GetContent(key);

            if (value is TContent1 valueT1)
            {
                return valueT1;
            }
            return default;
        }
        
        bool IContentDatabase.AddContent<TContent1>(ContentKey key, TContent1 value)
        {
            AssertType<TContent1>();

            if (value is TContent valueT)
            {
                return AddContent(key, valueT);
            }
            throw new UnsupportedContentTypeException(typeof(TContent1), new[] { typeof(TContent) });
        }

        static void AssertType<TContent1>() where TContent1 : notnull {
            if (typeof(TContent1) != typeof(TContent))
            {
                throw new UnsupportedContentTypeException(typeof(TContent1), new[] { typeof(TContent) });
            }
        }
    }
}
