using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace beltexam.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}
        public List<Idea> Ideas {get; set;}
        public List<Like> Likes {get; set;}
        [Required]
        [Display(Name = "Name:")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain letters only")]
        [MinLength(2, ErrorMessage = "First Name must be 2 characters or longer!")]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Alias:")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Alias must contain letters only")]
        [MinLength(2, ErrorMessage = "Alias must be 2 characters or longer!")]
        public string Alias { get; set; }


        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address!")]
        [Display(Name = "Email Address:")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        // Will not be mapped to your users table!
        [NotMapped]
        [Display(Name = "Password Confirm:")]
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }

    }
}