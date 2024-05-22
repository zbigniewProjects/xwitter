using System.ComponentModel.DataAnnotations;

namespace BlogProject.Contracts.Utils
{
    /// <summary>
    /// Just to prevent anyone from submitting any content that would contain single words that have absurd number of characters, that
    /// would destroy website design if for example there was a post with a word: aaaaaaaaaaaaaaaaaaaaaaaaaa....
    /// </summary>
    public class MaxWordLengthAttribute(int _maxLength) : ValidationAttribute
    {
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
