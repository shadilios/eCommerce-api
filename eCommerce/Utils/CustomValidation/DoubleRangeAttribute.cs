using System.ComponentModel.DataAnnotations;

namespace eCommerce.Utils.CustomValidation
{
    public class DoubleRangeAttribute : ValidationAttribute
    {
        public double Min { get; set; }
        public double Max { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                double doubleValue = (double)value;

                if (doubleValue < Min || doubleValue > Max)
                {
                    return new ValidationResult($"Value must be between {Min} and {Max}");
                }
            }

            return ValidationResult.Success;
        }
    }
}
