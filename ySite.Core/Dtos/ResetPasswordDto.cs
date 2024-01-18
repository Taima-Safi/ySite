using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.Dtos
{
    public class ResetPasswordDto
    {
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        [Required, Compare("NewPassword")]

        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
