using System.ComponentModel.DataAnnotations;

namespace BlogProject.Contracts.Utils
{
    public class MaxWordLengthAttribute : ValidationAttribute
    {
        private readonly int _maxLength;

        public MaxWordLengthAttribute(int maxLength)
        {
            _maxLength = maxLength;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string str)
            {
                var words = str.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    if (word.Length > _maxLength)
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
            }
            return ValidationResult.Success!;
        }
    }
}
