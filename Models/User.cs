using System;
using System.ComponentModel.DataAnnotations;

namespace LoginReg.Models
{
    public class User
    {
        [Required]
        [MinLength(3)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        public string LastName { get; set; }
        [Required]
        [MinLength(3)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MinLength(3)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MinLength(3)]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}