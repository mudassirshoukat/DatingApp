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
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder Builder)
        {
            

            // (UserLike Fluent Validations)
            base.OnModelCreating(Builder);
            Builder.Entity<UserLike>().HasKey(x => new { x.SourceUserId, x.TargetUserId });
            Builder.Entity<UserLike>()
                .HasOne(x => x.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(x => x.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);

            Builder.Entity<UserLike>()
               .HasOne(x => x.TargetUser)
               .WithMany(l => l.LikedByUsers)
               .HasForeignKey(x => x.TargetUserId)
               .OnDelete(DeleteBehavior.Cascade);


            // (Message) Fluent Validations

            Builder.Entity<Message>()
                .HasOne(x => x.Sender)
                .WithMany(x => x.MessagesSent)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            Builder.Entity<Message>()
                .HasOne(x => x.Recipient)
                .WithMany(x => x.MessagesRecieved)
                .HasForeignKey(x => x.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
            

            // (AppUser) Fluent Validations

            Builder.Entity<AppUser>().Property(x => x.DateOfBirth).HasConversion<DateOnlyConverter>();

        }



    }
}
