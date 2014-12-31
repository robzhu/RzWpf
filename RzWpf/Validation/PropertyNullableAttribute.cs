
namespace RzWpf.Validation
{
    /// <summary>
    /// This attribute indicates whether the decorated property can be null.
    /// </summary>
    /// <example>
    /// [PropertyNullable( IsNullable = false, ErrorMessage = "This property cannot be null." )]
    /// public object Value { get; set; }
    /// </example>
    public sealed class PropertyNullableAttribute : PropertyValidationAttribute
    {
        /// <summary>
        /// Gets or sets whether the decorated property can be null.
        /// </summary>
        public bool IsNullable { get; set; }

        protected override bool ValidateInternal( object toValidate, object context )
        {
            return IsNullable == ( null == toValidate );
        }
    }
}