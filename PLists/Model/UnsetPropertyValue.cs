using System;

namespace PLists.Model {
    public struct UnsetPropertyValue<TValue> : IPropertyValue<TValue> {
        public TValue Value {
            get => throw new InvalidOperationException("Cannot get the value of an unset property.");
            set => throw new InvalidOperationException("Cannot set the value of an unset property.");
        }
    }
}