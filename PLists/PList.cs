using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PLists.Exceptions;
using PLists.Model;

namespace PLists {
    /// <summary>
    /// Represents an in-memory, extensible, prototype-based property list.
    /// </summary>
    /// <typeparam name="TKey">The type of the property key.</typeparam>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    public class PList<TKey, TValue> : IPList<TKey, TValue> {
        private readonly Dictionary<TKey, IPropertyValue<TValue>> _properties = new();

        public PList(IPList<TKey, TValue> prototype) {
            Prototype = prototype;
        }

        public PList() {
        }

        /// <summary>
        /// Enumerates this <see cref="PList{TKey,TValue}"/>'s own and inherited but not overiden properties. 
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            foreach (var p in (IEnumerable<KeyValuePair<TKey, TValue>>) EnumerateOwnProperties()) {
                yield return p;
            }
            
            foreach (var p in (IEnumerable<KeyValuePair<TKey, TValue>>) EnumerateInheritedProperties()) {
                yield return p;
            }
        }

        private IEnumerator<KeyValuePair<TKey, TValue>> EnumerateOwnProperties() {
            var keyValuePairs = _properties
                .Where(pair => pair.Value is PropertyValue<TValue>)
                .Select(pair => new KeyValuePair<TKey, TValue>(pair.Key, pair.Value.Value));

            foreach (var keyValuePair in keyValuePairs) {
                yield return keyValuePair;
            }
        }

        /// <summary>
        /// Enumerates inherited properties which have not been overriden.
        /// </summary>
        private IEnumerator<KeyValuePair<TKey, TValue>> EnumerateInheritedProperties() {
            if (Prototype == null) {
                yield break;
            }
            
            foreach (var keyValuePair in Prototype.Where(pair => !_properties.ContainsKey(pair.Key))) {
                yield return keyValuePair;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        /// <summary>
        /// Removes all the properties defined for this <see cref="PList{TKey,TValue}"/>. Inherited properties will
        /// not be cleared.
        /// </summary>
        /// <remarks>
        /// If a inherited property is removed with <see cref="Remove(TKey)"/>, after calling <see cref="Clear"/>
        /// the value of that property will be the inherited one.
        /// </remarks>
        public void Clear() => _properties.Clear();

        public bool Contains(KeyValuePair<TKey, TValue> item) => TryGetValue(item.Key, out _);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            this.ToList().CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

        public int Count => _properties.Count + Prototype?.Count ?? 0;
        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value) {
            _properties.Add(key, PropertyValue<TValue>.Of(value));
        }

        public bool ContainsKey(TKey key) => TryGetValue(key, out _);

        public bool Remove(TKey key) {
            _properties[key] = new UnsetPropertyValue<TValue>();
            return true;
        }

        public bool TryGetValue(TKey key, out TValue? value) {
            value = default;

            if (!_properties.TryGetValue(key, out var propertyValue)) {
                return Prototype != null && Prototype.TryGetValue(key, out value);
            }
            
            if (propertyValue is UnsetPropertyValue<TValue>) {
                return false;
            }
                
            value = propertyValue.Value;
            return true;
        }

        public TValue this[TKey key] {
            get => (TryGetValue(key, out var value) 
                ? value
                : throw new PropertyNotFoundException<TKey>(key))!;
            set => _properties[key] = PropertyValue<TValue>.Of(value);
        }

        public ICollection<TKey> Keys { get; }
        public ICollection<TValue> Values { get; }
        public IPList<TKey, TValue>? Prototype { get; }
    }
}