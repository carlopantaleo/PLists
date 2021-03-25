using System.Collections.Generic;

namespace PLists {
    public interface IPList<TKey, TValue> : IDictionary<TKey, TValue> {
        /// <summary>
        /// The prototype from which this <see cref="IPList{TKey,TValue}"/>, or null if this
        /// <see cref="IPList{TKey,TValue}"/> is not derived from any other.
        /// </summary>
        IPList<TKey, TValue>? Prototype { get; }
    }
}