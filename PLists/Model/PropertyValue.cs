using System;

namespace PLists.Model {
    /// <summary>
    /// A concrete property value wrapper.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    internal struct PropertyValue<TValue> : IPropertyValue<TValue> {
        private TValue _value;

        public TValue Value {
            get => _value;
            set => _value = value ?? 
                            throw new NullReferenceException("Cannot set a null property value. If you want " +
                                                             "to unset the value of a property, please use " +
                                                             "IPList<TKey, TValue>.Unset().");
        }
    }
}