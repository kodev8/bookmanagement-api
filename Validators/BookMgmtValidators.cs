using WebApplication1.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Validators
{
    // public class NoHorrorAttribute : ValidationAttribute
    // {
    //     protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    //     {
    //         var book = (Book)validationContext.ObjectInstance;
    //         if (book.Genres != null && book.Genres.Contains(Genre.Horror))
    //         {
    //             return new ValidationResult("We don't sell horror books here");
    //         }
    //         return ValidationResult.Success;
    //     }
    // }


    public class NoSpecialCharsAttribute: ValidationAttribute
    {
        public string AllowableChars { get; set; } = "";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            
            if (value == null || ((string)value).Any(ch => !char.IsLetter(ch) && !AllowableChars.Contains(ch)))
            {
                return new ValidationResult("This fieldshould not contain numbers or special chars");
            }
                return ValidationResult.Success;
        }
    }

    internal class PasswordValidation 
    {
        public static bool runValidation(string password) {
            return hasUppercase(password) && hasLowercase(password) && hasNumber(password) && hasSpecialChar(password);
        }

        private static bool hasUppercase(string password) {
            return password.Any(char.IsUpper);
        }

        private static bool hasLowercase(string password) {
            return password.Any(char.IsLower);
        }

        private static bool hasNumber(string password) {
            return password.Any(char.IsDigit);
        }

        private static bool hasSpecialChar(string password) {
            return password.Any(ch => !char.IsLetterOrDigit(ch));
        }
    }

    public class PasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || !PasswordValidation.runValidation((string)value))
            {
                return new ValidationResult("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character");
            }
            return ValidationResult.Success;
        }
    }
}

