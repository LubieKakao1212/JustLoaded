using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aimless.ModLoader.Content.Database.Execeptions
{
    public class DatabaseLockedException : ApplicationException
    {
        public DatabaseLockedException() : base("Cannot add content to a locked ContentDatabase") { }
    }
}
