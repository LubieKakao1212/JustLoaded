using System.Security.AccessControl;
using Aimless.ModLoader.Util.Extensions;

namespace Aimless.ModLoader.Util.Algorithm
{
    public class TopoSorter<T> where T : notnull
    {
        private readonly HashSet<T> _elements;
        private readonly Dictionary<T, int> _dependencyCounter;
        /// <summary>
        /// Value is a Set of elements that depend on Key
        /// </summary>
        private readonly Dictionary<T, HashSet<T>> _dependants;

        public TopoSorter(int capacity = 0) 
        {
            _elements = new(capacity);
            _dependencyCounter = new(capacity);
            _dependants = new(capacity);
        }

        public void AddElement(T value)
        {
            _elements.Add(value);
            _dependencyCounter.Add(value, 0);
            _dependants.Add(value, new HashSet<T>());
        }

        /// <summary>
        /// Makes <paramref name="from"/> depend on <paramref name="to"/>
        /// </summary>
        public void AddDependency(T from, T to)
        {
            if (!_elements.Contains(from) || !_elements.Contains(to))
            {
                //TODO Exception
                throw new ApplicationException();
            }
            _dependencyCounter[from]++;
            _dependants.AddNested(to, from);
        }
        
        public List<T> Sort()
        {
            var queue = new Queue<T>();
            var dependencyCounter = new Dictionary<T, int>(this._dependencyCounter);
            var result = new List<T>(_elements.Count);

            foreach (var element in _elements)
            {
                if (dependencyCounter[element] == 0)
                {
                    queue.Enqueue(element);
                }
            }

            while (queue.TryDequeue(out var element))
            {
                result.Add(element);
                foreach (var dependant in _dependants[element])
                {
                    var depCount = --dependencyCounter[dependant];
                    if (depCount == 0)
                    {
                        queue.Enqueue(dependant);
                        continue;
                    }
                    if (depCount < 0)
                    {
                        //TODO Add proper exception
                        throw new Exception();
                    }
                }
            }

            if (result.Count != _elements.Count)
            {
                throw new ApplicationException("Dependency cycle detected");
            }

            return result;
        }

        public void ClearDependencies() {
            foreach (var element in _elements) {
                _dependencyCounter[element] = 0;
            }
            _dependants.ClearNested<T, T, HashSet<T>>();
        }
    }
}
