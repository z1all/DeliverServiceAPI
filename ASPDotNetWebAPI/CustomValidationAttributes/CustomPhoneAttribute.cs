using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ASPDotNetWebAPI.CustomValidationAttributes
{
    public class CustomPhoneAttribute : ValidationAttribute
    {
        public bool Nullable { get; set; } = false;

        public override bool IsValid(object? value)
        {
            if (Nullable && value == null)
            {
                return true;
            }

            bool isCorrect = false;
            if (value is string phone)
            {
                if (!Regex.IsMatch(phone, @"^(\+7|8)[0-9]{10}$"))
                {
                    ErrorMessage = "The phone number must start with '+7' or '8' and have 11 digits.";
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
