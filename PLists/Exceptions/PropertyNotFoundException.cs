using System;

namespace PLists.Exceptions {
    public class PropertyNotFoundException<TKey> : Exception {
        public TKey PropertyKey { get; }
        public PropertyNotFoundException(TKey key, TKey propertyKey) : base($"Property with key '{key}' not found.") {
            PropertyKey = propertyKey;
        }
    }
}