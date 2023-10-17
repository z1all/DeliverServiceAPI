using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ASPDotNetWebAPI.CustomValidationAttributes
{
    public class CustomPhoneAttribute : ValidationAttribute
    {
        private readonly bool nullable;

        public CustomPhoneAttribute(bool nullable)
        {
            this.nullable = nullable;
        }

        public override bool IsValid(object? value)
        {
            if (nullable && value == null)
            {
                return true;
            }

            if(value is string phone)
            {
                if(Regex.IsMatch(phone, @"^(\\+7|8)[0-9]{10}$"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
