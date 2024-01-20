using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.Helper
{
    public class ValidationResult
    {
        public bool  IsValid { get; set; }
        public string Message { get; set; }

        public ValidationResult(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message;
        }

        public static ValidationResult Success(string message)
        {
            return new ValidationResult(true, message);
        }

        public static ValidationResult Fail(string message)
        {
            return new ValidationResult(false, message);
        }

    }
}
