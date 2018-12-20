using Microsoft.EntityFrameworkCore;

namespace beltexam.Models
{
    public class BeltContext : DbContext
    {
        public BeltContext(DbContextOptions<BeltContext> options) : base(options) {}
        public  DbSet<User> Users { get; set;}
        public  DbSet<Idea> Ideas { get; set;}
        public DbSet<Like> Likes {get; set;}
    }
}