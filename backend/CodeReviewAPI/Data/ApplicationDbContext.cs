using CodeReviewAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeReviewAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CodeSubmission> CodeSubmissions { get; set; }
        public DbSet<CodeReview> CodeReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<CodeSubmission>()
                .HasOne(s => s.User)
                .WithMany(u => u.Submissions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CodeReview>()
                .HasOne(r => r.CodeSubmission)
                .WithMany(s => s.Reviews)
                .HasForeignKey(r => r.CodeSubmissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
