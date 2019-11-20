using Matcha.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactBook
{
    public class IsNotNullOrEmptyRule : ValidationRule<string>
    {
        public IsNotNullOrEmptyRule(string propertyName) : base(propertyName)
        {
            ValidationMessage = $"{propertyName} should not be empty!";
        }

        protected override bool CheckValue(string value)
        {
            return IsValid = !string.IsNullOrWhiteSpace(value);
        }
    }
}
