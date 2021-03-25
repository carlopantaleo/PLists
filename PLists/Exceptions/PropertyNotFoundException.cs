using System;

namespace PLists.Exceptions {
    public class PropertyNotFoundException<TKey> : Exception {
        public TKey PropertyKey { get; }
        public PropertyNotFoundException(TKey propertyKey) : base($"Property with key '{propertyKey}' not found.") {
            PropertyKey = propertyKey;
        }
    }
}