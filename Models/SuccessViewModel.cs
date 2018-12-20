using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace beltexam.Models
{
        public class SuccessViewModel
        {
        public Idea idea{get; set;}
        public Like like{get; set;}
        public User user{get; set;}
        }
}