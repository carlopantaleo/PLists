namespace PLists.Model {
    /// <summary>
    /// Wraps a property value.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    internal interface IPropertyValue<TValue> {
        TValue Value { get; set; }
    }
}