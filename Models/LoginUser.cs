using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace beltexam.Models
{
    public class LoginUser
    {
        [Display(Name = "Email:")]
        [EmailAddress(ErrorMessage = "Invalid Email Address!")]
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Password:")]
        [MinLength(1, ErrorMessage = "Password must be 8 characters or longer!")]
        public string Password { get; set; }
    }
}