using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace beltexam.Models
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }
        public int UserId {get; set;}
        public User User{get; set;}

        public int IdeaId{get; set;}
        public Idea Idea{get; set;}
        [NotMapped]
        public int Count{get; set;}
    }
}