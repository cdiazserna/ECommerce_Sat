using System.ComponentModel.DataAnnotations;

namespace ECommerce_Sat.Utilities
{
    public class NonEmptyGuidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            if (!(value is Guid))
            {
                return new ValidationResult("El valor proporcionado no es un Guid.");
            }

            Guid guidValue = (Guid)value;
            Guid zeroGuid = new Guid("00000000-0000-0000-0000-000000000000");

            if (guidValue == Guid.Empty || guidValue == zeroGuid)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
