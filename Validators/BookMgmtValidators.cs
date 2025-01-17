using WebApplication1.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Validators
{
    public class NoHorrorAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var book = (Book)validationContext.ObjectInstance;
            if (book.Genres != null && book.Genres.Contains(Genre.Horror))
            {
                return new ValidationResult("We don't sell horror books here");
            }
            return ValidationResult.Success;
        }
    }


    public class NoSpecialCharsAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //var book = (Book)validationContext.ObjectInstance;
            // check if author name is contains numbers or special characters
            //if (book.Author != null && book.Author.Any(ch => !char.IsLetter(ch) && ch != ' '))
            if (value != null && ((string)value).Any(ch => !char.IsLetter(ch) && ch != ' '))
            {
                return new ValidationResult("Author name should not contain numbers or special chars");
            }
                return ValidationResult.Success;
        }
    }

}
