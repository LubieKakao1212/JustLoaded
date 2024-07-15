using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustLoaded.Content.Database.Extensions
{
    public static partial class GlobalDatabases
    {
        /// <exception cref="ApplicationException">when no MasterDatabase was set</exception>
        public static MasterDatabase Master
        {
            get
            {
                return _master ?? throw new NullReferenceException("Global MasterDatabase not set");
            }
        }

        private static MasterDatabase? _master = null;

        public static void SetGlobal(this MasterDatabase master, bool force = false)
        {
            if (_master != null && !force)
            {
                throw new ApplicationException("Global MasterDatabase was already set");
            }
            _master = master;
        }
    }
}
