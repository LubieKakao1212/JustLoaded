using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aimless.ModLoader.Content.Database
{
    internal class ContentRegistrationException : Exception
    {
        public ContentRegistrationException(string message) : base(message)
        {
        
        }
    }
}
