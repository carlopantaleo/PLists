using System.Collections.Generic;

namespace PLists {
    public interface IPList<TKey, TValue> : IDictionary<TKey, TValue> {
        /// <summary>
        /// The prototype from which this <see cref="IPList{TKey,TValue}"/>, or null if this
        /// <see cref="IPList{TKey,TValue}"/> is not derived from any other.
        /// </summary>
        IPList<TKey, TValue>? Prototype { get; }

        /// <summary>
        /// Unsets the property with the given key.
        /// </summary>
        /// <param name="key">The key of the property to unset.</param>
        void Unset(TKey key);
    }
}