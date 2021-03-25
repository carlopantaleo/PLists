using System;
using System.Collections;
using System.Collections.Generic;

namespace PLists {
    /// <summary>
    /// Represents an in-memory, extensible, prototype-based property list.
    /// </summary>
    /// <typeparam name="TKey">The type of the property key.</typeparam>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    public class PList<TKey, TValue> : IPList<TKey, TValue> {
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item) {
            throw new NotImplementedException();
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) {
            throw new NotImplementedException();
        }

        public int Count { get; }
        public bool IsReadOnly { get; }
        public void Add(TKey key, TValue value) {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key) {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key) {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value) {
            throw new NotImplementedException();
        }

        public TValue this[TKey key] {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public ICollection<TKey> Keys { get; }
        public ICollection<TValue> Values { get; }
        public IPList<TKey, TValue>? Prototype { get; }
        public void Unset(TKey key) {
            throw new NotImplementedException();
        }
    }
}