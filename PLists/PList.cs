using System;
using System.Collections;
using System.Collections.Generic;
using PLists.Exceptions;
using PLists.Model;

namespace PLists {
    /// <summary>
    /// Represents an in-memory, extensible, prototype-based property list.
    /// </summary>
    /// <typeparam name="TKey">The type of the property key.</typeparam>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    public class PList<TKey, TValue> : IPList<TKey, TValue> {
        private readonly Dictionary<TKey, IPropertyValue<TValue>> _properties = 
            new Dictionary<TKey, IPropertyValue<TValue>>();

        public PList(IPList<TKey, TValue> prototype) {
            Prototype = prototype;
        }

        public PList() {
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            set => Add(key, value);
        }

        public ICollection<TKey> Keys { get; }
        public ICollection<TValue> Values { get; }
        public IPList<TKey, TValue>? Prototype { get; }
    }
}