using API.Entities;
using API.Services;
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
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder Builder)
        {
      //      Builder.Entity<AppUser>().HasData(
      //    new AppUser
      //    {
      //        id = 1,
      //        UserName = "SampleUser",
      //        PasswordHash = new byte[] { 0x1, 0x2, 0x3, 0x4 },
      //        PasswordSalt = new byte[] { 0x5, 0x6, 0x7, 0x8 },
      //        DateOfBirth = new DateOnly(1990, 1, 1),
      //        KnownAs = "John Doe",
      //        LastActive = DateTime.UtcNow,
      //        Created = DateTime.UtcNow,
      //        Gender = "Male",
      //        Introduction = "Hello, I'm John Doe.",
      //        LookingFor = "Looking for someone special.",
      //        Interests = "Programming, Reading",
      //        City = "Sample City",
      //        Country = "Sample Country"
      //    }
      //);

            Builder.Entity<AppUser>().Property(x => x.DateOfBirth).HasConversion<DateOnlyConverter>();

        }



    }
}
