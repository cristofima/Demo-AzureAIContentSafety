using AzureAIContentSafety.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AzureAIContentSafety.API.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)      
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Text)
                    .HasMaxLength(1000)
                    .IsRequired();

                entity.Property(e => e.ImagePath)
                    .IsRequired(false);

                entity.Property(e => e.TextRequiresModeration)
                    .IsRequired();

                entity.Property(e => e.TextIsHarmful)
                   .IsRequired();

                entity.Property(e => e.TextHateSeverity)
                   .IsRequired();

                entity.Property(e => e.TextSelfHarmSeverity)
                   .IsRequired();

                entity.Property(e => e.TextSexualSeverity)
                   .IsRequired();

                entity.Property(e => e.TextViolenceSeverity)
                   .IsRequired();

                entity.Property(e => e.ImageRequiresModeration)
                   .IsRequired();

                entity.Property(e => e.ImageIsHarmful)
                   .IsRequired();

                entity.Property(e => e.ImageHateSeverity)
                   .IsRequired();

                entity.Property(e => e.ImageSelfHarmSeverity)
                   .IsRequired();

                entity.Property(e => e.ImageSexualSeverity)
                   .IsRequired();

                entity.Property(e => e.ImageViolenceSeverity)
                   .IsRequired();

                entity.Property(e => e.CreatedAt)
                   .IsRequired();

                entity.Property(e => e.LastUpdatedAt)
                   .IsRequired(false);
            });
        }
    }
}