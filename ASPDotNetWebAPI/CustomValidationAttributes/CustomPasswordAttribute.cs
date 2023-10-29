using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ASPDotNetWebAPI.CustomValidationAttributes
{
    public class CustomPasswordAttribute : ValidationAttribute
    {
        public bool Nullable { get; set; } = false;

        public override bool IsValid(object? value)
        {
            if (Nullable && value == null)
            {
                return true;
            }

            bool isCorrect = false;
            if (value is string password)
            {
                if (!Regex.IsMatch(password, @"[0-9]") || !Regex.IsMatch(password, @"[A-Z]"))
                {
                    ErrorMessage = "The password must contain at least one digit and one capital letter.";
                }
                else
                {
                    isCorrect = true;
                }
            }

            return isCorrect;
        }
    }
}
