using System.Drawing;
using JustLoaded.Util.Extensions;
using JustLoaded.Content;
using JustLoaded.Util;
using JustLoaded.Util.Algorithm;

namespace JustLoaded.Core.Loading;

public class OrderedResolver<TContent> where TContent : notnull {

    private readonly Dictionary<ContentKey, HashSet<ContentKey>> _deps = new();
    private readonly Dictionary<ContentKey, TContent> _elements = new();
    
    /// <summary>
    /// Remember to call <see cref="Registration.Register"/> on the returned registration
    /// </summary>
    /// <returns>A new <see cref="Registration"/> object</returns>
    public Registration New(ContentKey key, TContent value) {
        if (_elements.ContainsKey(key)) {
            //TODO exception
            throw new Exception($"Duplicate id {key}");
        }
        return new Registration(this, key, value);
    }

    public IEnumerable<KeyValuePair<ContentKey, TContent>> Resolve() {
        var sorter = new TopoSorter<ContentKey>();

        foreach (var element in _elements) {
            sorter.AddElement(element.Key);
        }

        foreach (var source in _deps) {
            if (!_elements.ContainsKey(source.Key)) {
                continue;
            }

            foreach (var dep in source.Value) {
                if (!_elements.ContainsKey(dep)) {
                    continue;
                }
                sorter.AddDependency(source.Key, dep);
            }
        }

        return sorter.Sort().Select((key) => new KeyValuePair<ContentKey, TContent>(key, _elements[key]));
    }
    
    private void Add(ContentKey key, TContent value) {
        _elements.Add(key, value);
        _deps.GetValueOrSetDefaultLazy(key, () => new HashSet<ContentKey>());
    }

    private void AddDependency(ContentKey from, ContentKey to) {
        _deps.AddNested(from, to);
    }
    
    public class Registration {
        
        private readonly ContentKey _key;
        private readonly TContent _value;
        private readonly OrderedResolver<TContent> _resolver;
        private readonly Dictionary<ContentKey, Order> _order = new();
        
        internal Registration(OrderedResolver<TContent> resolver, ContentKey key, TContent value) {
            this._key = key;
            this._value = value;
            this._resolver = resolver;
        }

        public Registration WithOrder(ContentKey relativeTo, Order order) {
            _order.Add(relativeTo, order);
            return this;
        }

        public void Register() {
            _resolver.Add(_key, _value);
            foreach (var order in _order) {
                switch (order.Value) {
                    case Order.After:
                        _resolver.AddDependency(_key, order.Key);
                        continue;
                    case Order.Before:
                        _resolver.AddDependency(order.Key, _key);
                        continue;
                    default:
                        //TODO use logger
                        Console.Out.WriteLine($"Using order { Order.Any } does nothing for order only dependencies");
                        continue;
                }
            }
        }
    }
}