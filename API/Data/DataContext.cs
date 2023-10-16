using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder Builder)
        {
            Builder.Entity<AppUser>().HasData(
                new AppUser { id=1, UserName = "Bob" },
                new AppUser { id = 2, UserName = "Charlie" },
                new AppUser { id = 3, UserName = "Ban" },
                new AppUser {id=4, UserName = "Lara" }
                );


        }

    }
}
