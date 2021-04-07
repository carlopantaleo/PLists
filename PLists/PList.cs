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
    public class PList<TKey, TValue> : IPList<TKey, TValue?> {
        private readonly Dictionary<TKey, IPropertyValue<TValue>> _properties = new();
        
        /// <summary>
        /// All the keys of own and inherited properties.
        /// </summary>
        public ICollection<TKey> Keys => this.Select(pair => pair.Key).ToList();
        
        /// <summary>
        /// All the values of own and inherited properties.
        /// </summary>
        public ICollection<TValue?> Values => this.Select(pair => pair.Value).ToList();
        
        /// <summary>
        /// The prototype from which this <see cref="PList{TKey,TValue}"/> is inheriting from.
        /// </summary>
        public IPList<TKey, TValue?>? Prototype { get; }

        /// <summary>
        /// Creates a new <see cref="PList{TKey,TValue}"/> based on the given prototype.
        /// </summary>
        /// <param name="prototype">The prototype of the <see cref="PList{TKey,TValue}"/> to create.</param>
        public PList(IPList<TKey, TValue?> prototype) {
            Prototype = prototype;
        }

        /// <summary>
        /// Creates a new <see cref="PList{TKey,TValue}"/> by cloning the given <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/> to clone.</param>
        /// <remarks>This <see cref="PList{TKey,TValue}"/> will not have a <see cref="Prototype"/>.</remarks>
        public PList(IDictionary<TKey, TValue?> dictionary) {
            foreach (var (key, value) in dictionary) {
                _properties.Add(key, PropertyValue<TValue?>.Of(value));
            }
        }

        public PList() {
        }

        /// <summary>
        /// Enumerates this <see cref="PList{TKey,TValue}"/>'s own and inherited but not overriden properties. 
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue?>> GetEnumerator() {
            foreach (var p in EnumerateOwnProperties()) {
                yield return p;
            }
            
            foreach (var p in EnumerateInheritedProperties()) {
                yield return p;
            }
        }

        private IEnumerable<KeyValuePair<TKey, TValue?>> EnumerateOwnProperties() {
            var keyValuePairs = _properties
                .Where(pair => pair.Value is PropertyValue<TValue>)
                .Select(pair => new KeyValuePair<TKey, TValue?>(pair.Key, pair.Value.Value));

            foreach (var keyValuePair in keyValuePairs) {
                yield return keyValuePair;
            }
        }

        /// <summary>
        /// Enumerates inherited properties which have not been overriden.
        /// </summary>
        private IEnumerable<KeyValuePair<TKey, TValue?>> EnumerateInheritedProperties() {
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

        public void Add(KeyValuePair<TKey, TValue?> item) => Add(item.Key, item.Value);

        /// <summary>
        /// Removes all the properties defined for this <see cref="PList{TKey,TValue}"/>. Inherited properties will
        /// not be cleared.
        /// </summary>
        /// <remarks>
        /// If a inherited property is removed with <see cref="Remove(TKey)"/>, after calling <see cref="Clear"/>
        /// the value of that property will be the inherited one.
        /// </remarks>
        public void Clear() => _properties.Clear();

        public bool Contains(KeyValuePair<TKey, TValue?> item) => 
            TryGetValue(item.Key, out var value) && value != null && value.Equals(item.Value);

        public void CopyTo(KeyValuePair<TKey, TValue?>[] array, int arrayIndex) => 
            this.Select(pair => pair).ToList().CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue?> item) => Remove(item.Key);

        public bool Remove(TKey key) {
            _properties[key] = new UnsetPropertyValue<TValue>();
            return true;
        }

        /// <summary>
        /// The count of this <see cref="PList{TKey,TValue}"/> properties, including inherited ones.
        /// </summary>
        public int Count =>
            // Performing a Select() enumerates this PList and consolidates hierarchy.
            this.Select(_ => 1).Count();

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue? value) {
            _properties.Add(key, PropertyValue<TValue>.Of(value));
        }

        public bool ContainsKey(TKey key) => TryGetValue(key, out _);

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

        public TValue? this[TKey key] {
            get => (TryGetValue(key, out var value) 
                ? value
                : throw new PropertyNotFoundException<TKey>(key))!;
            set => _properties[key] = PropertyValue<TValue>.Of(value);
        }
    }
}