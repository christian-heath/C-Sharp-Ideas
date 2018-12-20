using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace beltexam.Models
{
    public class Idea
    {
        [Key]
        public int IdeaId { get; set; }
        public int UserId {get; set;}
        public User User{get; set;}
        public List<Like> Likes{get; set;}

        [Required(ErrorMessage = "No blank ideas!")]
        [MaxLength(255)]
        [Display(Name = "Idea:")]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}