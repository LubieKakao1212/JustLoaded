using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using JustLoaded.Content.Database.Exceptions;
using JustLoaded.Util;

namespace JustLoaded.Content.Database
{
    public interface IContentDatabase {
        
        delegate void ContentAddedCallback<TContent>(ContentKey key, ref TContent content) where TContent : notnull;
        event Action Locked;
        
        bool IsLocked { get; }
        Type[] SupportedContentTypes { get; }

        void Lock();
        
        /// <summary>
        /// </summary>
        /// <exception cref="UnsupportedContentTypeException">When this <see cref="IContentDatabase"/> does not support given <typeparamref name="TContent"/> content Type</exception>
        /// <returns>Requested Content</returns>
        TContent? GetContent<TContent>(ContentKey key) where TContent : notnull;
        
        /// <summary>
        /// </summary>
        /// <exception cref="UnsupportedContentTypeException">When this <see cref="IContentDatabase"/> does not support given <typeparamref name="TContent"/> content Type</exception>
        /// <exception cref="DatabaseLockedException">When this database is locked (<see cref="IsLocked"/>)</exception>
        /// <returns>True if element was successfully added, False if given key already exists</returns>
        void AddContent<TContent>(ContentKey key, TContent value) where TContent : notnull;
        
        /// <summary>
        /// Overload of <see cref="AddContent{TContent}"/> for use with reflection. <br/>
        /// If you are not using reflection and know the type at compile time, use <see cref="AddContent{TContent}"/>
        /// </summary>
        /// <inheritdoc cref="AddContent{TContent}"/>
        void AddContent(ContentKey key, Type type, object value);

        /// <summary>
        /// </summary>
        /// <exception cref="UnsupportedContentTypeException">When this <see cref="IContentDatabase"/> does not support given <typeparamref name="TContent"/> content Type</exception>
        /// <exception cref="DatabaseLockedException">When this database is locked (<see cref="IsLocked"/>)</exception>
        /// <returns>True if element was successfully added, False if any error was encountered during method execution</returns>
        bool TryAddContent<TContent>(ContentKey key, TContent value) where TContent : notnull;
        
        /// <summary>
        /// Overload of <see cref="TryAddContent{TContent}"/> for use with reflection. <br/>
        /// If you are not using reflection and know the type at compile time, use <see cref="TryAddContent{TContent}"/>
        /// </summary>
        /// <inheritdoc cref="TryAddContent{TContent}"/>
        bool TryAddContent(ContentKey key, Type type, object value);
        
        void RegisterContentAddedCallback<TContent>(ContentAddedCallback<TContent> callback) where TContent : notnull;
        
        IEnumerable<ContentKey> GetContentKeys<TContent>() where TContent : notnull;
        IEnumerable<KeyValuePair<ContentKey, TContent>> GetContentEntries<TContent>() where TContent : notnull;
        IEnumerable<TContent> GetContentValues<TContent>() where TContent : notnull;
    
        bool IsTypeSupported(Type type)
        {
            return SupportedContentTypes.Contains(type);
        }
    }

    public interface IContentDatabase<TContent> : IContentDatabase where TContent : notnull {
        
        event ContentAddedCallback<TContent> ContentAdded;
        
        IEnumerable<TContent> ContentValues { get; }
        IEnumerable<KeyValuePair<ContentKey, TContent>> ContentEntries { get; }
        IEnumerable<ContentKey> ContentKeys { get; }

        Type[] IContentDatabase.SupportedContentTypes => new[] { typeof(TContent) };

        void IContentDatabase.RegisterContentAddedCallback<TContent1>(ContentAddedCallback<TContent1> callback) {
            AssertType<TContent1>();
            ContentAdded += callback as ContentAddedCallback<TContent>;
        }

        IEnumerable<ContentKey> IContentDatabase.GetContentKeys<TContent1>() {
            AssertType<TContent1>();
            return ContentKeys;
        }

        IEnumerable<KeyValuePair<ContentKey, TContent1>> IContentDatabase.GetContentEntries<TContent1>() {
            AssertType<TContent1>();
            //Should work?
            return (IEnumerable<KeyValuePair<ContentKey, TContent1>>)ContentEntries;
        }

        IEnumerable<TContent1> IContentDatabase.GetContentValues<TContent1>() {
            AssertType<TContent1>();
            //Should work?
            return (IEnumerable<TContent1>)ContentValues;
        }

        TContent? GetContent(ContentKey key);
        
        void AddContent(ContentKey key, TContent value);
        
        bool TryAddContent(ContentKey key, TContent value);

        [return: MaybeNull]
        TContent1 IContentDatabase.GetContent<TContent1>(ContentKey key)
        {
            AssertType<TContent1>();
            var value = GetContent(key);

            if (value is TContent1 valueT1)
            {
                return valueT1;
            }
            return default;
        }
        
        void IContentDatabase.AddContent<TContent1>(ContentKey key, TContent1 value)
        {
            AssertType<TContent1>();

            if (value is TContent valueT)
            {
                AddContent(key, valueT);
            }
            throw new UnsupportedContentTypeException(typeof(TContent1), new[] { typeof(TContent) });
        }

        static void AssertType<TContent1>() where TContent1 : notnull
        {
            if (!typeof(TContent).IsAssignableFrom(typeof(TContent1)))
            {
                throw new UnsupportedContentTypeException(typeof(TContent1), new[] { typeof(TContent) });
            }
        }
    }
}
