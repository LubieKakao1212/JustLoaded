using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Aimless.ModLoader.Content.Database
{
    public class MasterDatabase : ContentDatabase<IContentDatabase>
    {
        private static MasterDatabase? Instance = null;
        private Dictionary<Type, IContentDatabase> valuesByType = new();

        public MasterDatabase() { 
            
        }

        public IContentDatabase? GetByType(Type type)
        {
            if (!valuesByType.TryGetValue(type, out var val))
            {
                return null;
            }
            return val;
        }

        public override 

        public bool SetGlobal(bool force = false)
        {
            if (Instance != null && !force)
            {
                return false;
            }
            Instance = this;
            return true;
        }
    }
}
