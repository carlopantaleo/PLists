using System;

namespace PLists.Model {
    /// <summary>
    /// A concrete property value representing an unset property.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    internal struct UnsetPropertyValue<TValue> : IPropertyValue<TValue> {
        public TValue? Value {
            get => throw new InvalidOperationException("Cannot get the value of an unset property.");
            set => throw new InvalidOperationException("Cannot set the value of an unset property.");
        }
    }
}