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

        public readonly int hash;

        public ContentKey(string source, string id)
        {
            this.source = source;
            this.id = id;
            this.hash = ComputeHash(source, id);
        }

        public ContentKey(string key)
        {
            var split = key.Split(':');
            this.source = split[0];
            this.id = split[1];
            this.hash = ComputeHash(source, id);
        }

        public readonly override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is ContentKey id && Equals(id);
        }

        public readonly bool Equals(ContentKey other)
        {
            return hash == other.hash && source.Equals(other.source) && id.Equals(other.id);
        }

        public readonly override int GetHashCode()
        {
            return hash;
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

        private static int ComputeHash(string source, string id)
        {
            return HashCode.Combine(source.GetHashCode(), id.GetHashCode());
        }
    }
}
