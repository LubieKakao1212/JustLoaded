using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Aimless.ModLoader.Content.Database
{
    /// <summary>
    /// MasterDatabase contains all <see cref="ContentDatabase{T}"/> instances <br/>
    /// Contains a reference to itself under key "core:database"
    /// </summary>
    public class MasterDatabase : ContentDatabase<IContentDatabase>
    {
        private static MasterDatabase? Instance = null;
        private Dictionary<Type, IContentDatabase> valuesByType = new();

        public MasterDatabase() {
            AddContent(new ContentKey("core", "database"), typeof(IContentDatabase), this);
        }

        public IContentDatabase? GetByType(Type type)
        {
            if (!valuesByType.TryGetValue(type, out var val))
            {
                return null;
            }
            return val;
        }

        public bool AddContent(ContentKey key, Type type, IContentDatabase value)
        {
            if (valuesByType.ContainsKey(type))
            {
                return false;
            }
            if (!base.AddContent(key, value))
            {
                return false;
            }
            valuesByType.Add(type, value);
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
            if (Instance == null)
            {
                throw new ApplicationException("No MasterDatabase instance");
            }

            bool flag = false;
            switch (registrationType)
            {
                case DBRegistrationType.Any:
                    if (type != null)
                    {
                        flag = Instance.AddContent(key, type, database);
                    }
                    goto case DBRegistrationType.Secondary;
                case DBRegistrationType.Main:
                    if (type == null)
                    {
                        throw new ContentRegistrationException($"Failed to register Main database [{key}] for type null");
                    }
                    if (!Instance.AddContent(key, type, database))
                    {
                        throw new ContentRegistrationException($"Failed to register Main database [{key}] for type: {type}, key or type already set");
                    }
                    return;
                case DBRegistrationType.Secondary:
                    if (!flag)
                    {
                        Instance.AddContent(key, database);
                    }
                    break;
            }
        }
    }
}
