using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace LMPlatform.UI.Attributes
{
    public class GreaterThanAttribute : CompareAttribute
    {
        public GreaterThanAttribute(string otherProperty)
            : base(otherProperty)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(this.OtherProperty);
            if (property == null)
            {
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "Неизвестное свойство {0}", this.OtherProperty));
            }

            var otherValue = property.GetValue(validationContext.ObjectInstance, null);

            if (Comparer.DefaultInvariant.Compare(value, otherValue) > 0)
            {
                return null;
            }

            return new ValidationResult(validationContext.DisplayName);
        }
    }
}