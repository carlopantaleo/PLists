namespace PLists.Model {
    /// <summary>
    /// A concrete property value wrapper.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    internal struct PropertyValue<TValue> : IPropertyValue<TValue> {
        public TValue? Value { get; set; }

        public static PropertyValue<TValue> Of(TValue? value) => new() {Value = value};
    }
}