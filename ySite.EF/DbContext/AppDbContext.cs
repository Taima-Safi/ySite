using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ySite.EF.Entities;
using static System.Collections.Specialized.BitVector32;

namespace ySite.EF.DbContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PostModel> Posts { get; set; }
        public DbSet<ReactionModel> Reactions { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<FriendShipModel> FriendShips { get; set; }
        public DbSet<ReplayModel> Replays { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PostModel>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .IsRequired();


            builder.Entity<CommentModel>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.NoAction); // Change to Restrict or other appropriate action
            

            builder.Entity<ReactionModel>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Restrict); // Set the delete behavior to Restrict (NO ACTION)

            builder.Entity<FriendShipModel>()
            .HasOne(fs => fs.User)
            .WithMany()
            .HasForeignKey(fs => fs.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FriendShipModel>()
            .HasOne(fs => fs.Friend)
            .WithMany()
            .HasForeignKey(fs => fs.FriendId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PostModel>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<ReactionModel>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<CommentModel>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<ApplicationUser>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<ReplayModel>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<FriendShipModel>().HasQueryFilter(c => c.Status != FStatus.Declined);
        }
    }
}