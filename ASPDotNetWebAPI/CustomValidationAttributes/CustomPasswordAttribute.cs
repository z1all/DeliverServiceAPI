using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ASPDotNetWebAPI.CustomValidationAttributes
{
    public class CustomPasswordAttribute : ValidationAttribute
    {
        public bool Nullable { get; set; } = false;

        public override bool IsValid(object? value)
        {
            if(Nullable &&  value == null) 
            {
                return true;
            }

            if (value is string password)
            {
                if (!Regex.IsMatch(password, @"[A-Z]"))
                {
                    ErrorMessage = "Пароль должен содержать хотя бы одну заглавную букву.";
                    return false;
                }

                if (!Regex.IsMatch(password, @"[0-9]"))
                {
                    ErrorMessage = "Пароль должен содержать хотя бы одну цифру.";
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
