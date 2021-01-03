using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.DTO
{
    public class UserCredentials
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength: 100, ErrorMessage = "Email too long.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(200, MinimumLength = 8, ErrorMessage = "Password must contain 8 characters")]
        public string Password { get; set; }
    }
}
