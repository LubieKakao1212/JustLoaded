using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Aimless.ModLoader.Content.Database
{
    /// <summary>
    /// MasterDatabase contains all <see cref="IContentDatabase"/> instances <br/>
    /// Contains a reference to itself under key "core:database"
    /// </summary>
    public class MasterDatabase : ContentDatabase<IContentDatabase>
    {
        private static MasterDatabase? Instance = null;
        private Dictionary<Type, ContentKey> keysByType = new();

        public MasterDatabase() {
            AddContent(new ContentKey("core", "database"), typeof(IContentDatabase), this);
        }

        public ContentKey? KeyByContentType(Type type)
        {
            if (keysByType.TryGetValue(type, out var key))
            {
                return key;
            }
            return null;
        }

        public IContentDatabase? GetByContentType(Type type)
        {
            if (!keysByType.TryGetValue(type, out var key))
            {
                return null;
            }
            return GetContent(key);
        }

        public bool AddContent(ContentKey key, Type type, IContentDatabase value)
        {
            if (keysByType.ContainsKey(type))
            {
                return false;
            }
            if (!base.AddContent(key, value))
            {
                return false;
            }
            keysByType.Add(type, key);
            return true;
        }

        public bool SetGlobal(bool force = false)
        {
            if (Instance != null && !force)
            {
                return false;
            }
            Instance = this;
            return true;
        }

        /// <summary>
        /// Creates a new <see cref="ContentDatabase{TContent}"/> and registers it to <see cref="MasterDatabase.Instance"/>
        /// </summary>
        /// <param name="key">The <see cref="ContentKey"/> associated with the created database</param>
        /// <param name="registrationType"></param>
        /// <returns></returns>
        public static IContentDatabase<TContent> CreateDatabase<TContent>(ContentKey key, DBRegistrationType registrationType = DBRegistrationType.Any) where TContent : notnull 
        {
            var db = new ContentDatabase<TContent>();
            RegisterDatabase(key, typeof(TContent), db, registrationType);
            return db;
        }

        public static void RegisterDatabase(ContentKey key, Type? type, IContentDatabase database, DBRegistrationType registrationType = DBRegistrationType.Any)
        {
            var master = AssertMaster();
            if (type != null)
            {
                AssertContentType(database, type);
            }

            bool flag = false;
            switch (registrationType)
            {
                case DBRegistrationType.Any:
                    if (type != null)
                    {
                        flag = master.AddContent(key, type, database);
                    }
                    goto case DBRegistrationType.Secondary;
                case DBRegistrationType.Main:
                    if (type == null)
                    {
                        throw new ContentRegistrationException($"Failed to register Main database [{key}] for type null");
                    }
                    if (!master.AddContent(key, type, database))
                    {
                        throw new ContentRegistrationException($"Failed to register Main database [{key}] for type: {type}, key or type already set");
                    }
                    return;
                case DBRegistrationType.Secondary:
                    if (!flag)
                    {
                        master.AddContent(key, database);
                    }
                    break;
            }
        }

        //TODO excation type
        /// <exception cref="ApplicationException">when no database is found</exception>
        public static IContentDatabase GetDatabase<TContent>(ContentKey? key)
        {
            var master = AssertMaster();
            IContentDatabase? db = null;
            var contentType = typeof(TContent);
            if (key != null)
            {
                db = master.GetContent(key.Value);
            }
            else
            {
                db = master.GetByContentType(contentType);
            }
            if (db == null)
            {
                throw new ApplicationException();
            }
            AssertContentType(db, contentType);
            return db;
        }

        //TODO excation type
        /// <exception cref="ApplicationException">when no database is found </exception>
        public static IContentDatabase GetDatabase<TContent>()
        {
            return GetDatabase<TContent>(null);
        }

        private static MasterDatabase AssertMaster()
        {
            if (Instance == null)
            {
                throw new ApplicationException("No valid MasterDatabase Instance found");
            }
            return Instance;
        }

        private static void AssertContentType(IContentDatabase db, Type contentType)
        {
            if (!db.IsTypeSuported(contentType)) 
            { 
                throw new UnsupportedContentTypeException(contentType, db.SupportedContentTypes);
            }
        }
    }
}
