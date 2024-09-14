using System.ComponentModel.DataAnnotations;

namespace Estacionei.Validations
{
    public class ValidateEnumAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public ValidateEnumAttribute(Type enumType)
        {
            _enumType = enumType;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // deixando que o Required trate 
            }


            if (!Enum.IsDefined(_enumType,value ))
            {
                return new ValidationResult($"O {_enumType.Name} esta incorreto.");
            }


            return ValidationResult.Success;
        }
    }
}
