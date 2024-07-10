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

        public readonly string path;

        public readonly int hash;

        public ContentKey(string source, string path)
        {
            this.source = source;
            this.path = path;
            this.hash = ComputeHash(source, path);
        }

        public ContentKey(string key)
        {
            var split = key.Split(':');
            this.source = split[0];
            this.path = split[1];
            this.hash = ComputeHash(source, path);
        }

        public readonly override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is ContentKey id && Equals(id);
        }

        public readonly bool Equals(ContentKey other)
        {
            return hash == other.hash && source.Equals(other.source) && path.Equals(other.path);
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
            return source + ":" + path;
        }

        private static int ComputeHash(string source, string id)
        {
            return HashCode.Combine(source.GetHashCode(), id.GetHashCode());
        }
    }
}
