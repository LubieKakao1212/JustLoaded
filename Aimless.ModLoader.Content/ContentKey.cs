using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aimless.ModLoader.Content
{
    public readonly struct ContentKey : IEquatable<ContentKey>
    {
        public readonly string source;

        public readonly string id;

        public ContentKey(string source, string id)
        {
            this.source = source;
            this.id = id;
        }

        public ContentKey(string key)
        {
            var split = key.Split(':');
            source = split[0];
            id = split[1];
        }

        public readonly override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is ContentKey id && Equals(id);
        }

        public readonly bool Equals(ContentKey other)
        {
            return source.Equals(other.source) && id.Equals(other.id);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(source.GetHashCode(), id.GetHashCode());
        }

        public static bool operator ==(ContentKey left, ContentKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ContentKey left, ContentKey right)
        {
            return !(left == right);
        }

        public readonly override string ToString()
        {
            return source + ":" + id;
        }
    }
}
