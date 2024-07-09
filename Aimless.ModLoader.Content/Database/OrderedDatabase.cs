using Aimless.ModLoader.Util.Algorithm;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aimless.ModLoader.Content.Database
{
    /*public class OrderedDatabase<TContent> : ContentDatabase<TContent> where TContent : notnull
    {
        private readonly TopoSorter<ContentKey> keyOrderer = new();
        private readonly HashSet<Dependency> pendingRequired = new();
        private readonly HashSet<Dependency> pendingOptional = new();
        private readonly List<ContentKey> orderedKeys = new();
        private bool? valid;

        public OrderedDatabase()
        {

        }

        public void AddRequiredDependency(in ContentKey from, in ContentKey to, FromTo primary = FromTo.From)
        {
            AssertDependency(from, to, primary);
            var dep = new Dependency(from, to, primary);

            pendingRequired.Add(dep);
            MarkDIrty();
        }

        public void AddOptionalDependency(in ContentKey from, in ContentKey to, FromTo primary = FromTo.From)
        {
            AssertDependency(from, to, primary);

            var dep = new Dependency(from, to, primary);

            pendingOptional.Add(dep);
            MarkDIrty();
        }

        public override bool AddContent(ContentKey key, TContent value)
        {
            if (base.AddContent(key, value))
            {
                keyOrderer.AddElement(key);
                MarkDIrty();
                return true;
            }
            return false;
        }

        /// <param name="hard">If true and exception will be thrown upon failure</param>
        /// <returns></returns>
        public bool Validate(bool hard = false)
        {
            if (valid.HasValue && valid.Value)
            {
                return true;
            }

            bool flag = true;

            orderedKeys.Clear();
            keyOrderer.ClearDependencies();

            //TODO error tracing
            foreach (var dep in pendingRequired)
            {
                //Check if secondary is present
                if (dep.primary == FromTo.From)
                {
                    //Check if secondary is present
                    var valid = content.ContainsKey(dep.to);
                    flag &= valid;
                    if (!valid)
                    {
                        continue;
                    }
                }
                else
                {
                    //Check if secondary is present
                    var valid = content.ContainsKey(dep.from);
                    flag &= valid;
                    if (!valid)
                    {
                        continue;
                    }
                }
                keyOrderer.AddDependency(dep.from, dep.to);
            }

            foreach (var dep in pendingOptional)
            {

            }

            try
            {
                orderedKeys.AddRange(keyOrderer.Sort());
            }
            catch (ApplicationException e)
            {
                if (hard)
                {
                    throw;
                }

                //TODO log
            }
            return false;
        }

        private void MarkDIrty()
        {
            this.valid = null;
        }

        private struct Dependency : IEquatable<Dependency>
        {
            public ContentKey from;
            public ContentKey to;
            public FromTo primary;

            public Dependency(in ContentKey from, in ContentKey to, FromTo primary)
            {
                this.from = from;
                this.to = to;
                this.primary = primary;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(from, to);
            }

            public override bool Equals([NotNullWhen(true)] object? obj)
            {
                return obj is Dependency dep && Equals(dep);
            }

            public bool Equals(Dependency other)
            {
                return
                    (from.Equals(other.from) && to.Equals(other.to));*//* ||
                    (from.Equals(other.to) && to.Equals(other.from));*//*
            }
        }

        private void AssertDependency(in ContentKey from, in ContentKey to, FromTo primary = FromTo.From)
        {
            if (primary == FromTo.From)
            {
                //Throws when "from" is not present
                var dummy = content[from];
            }
            else
            {
                //Throws when "to" is not present
                var dummy = content[to];
            }
        }

        public enum FromTo 
        { 
            From = 0,
            To = 1
        }
    }*/
}
