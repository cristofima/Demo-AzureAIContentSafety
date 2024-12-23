using AzureAIContentSafety.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AzureAIContentSafety.API.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

                entity.Property(e => e.Text).HasMaxLength(1000).IsRequired(false);

                entity.Property(e => e.ImagePath).IsRequired(false);

                entity.Property(e => e.TextRequiresModeration).HasDefaultValue(false).IsRequired();

                entity.Property(e => e.TextIsHarmful).HasDefaultValue(false).IsRequired();

                entity.Property(e => e.TextHateSeverity).HasDefaultValue(0).IsRequired();

                entity.Property(e => e.TextSelfHarmSeverity).HasDefaultValue(0).IsRequired();

                entity.Property(e => e.TextSexualSeverity).HasDefaultValue(0).IsRequired();

                entity.Property(e => e.TextViolenceSeverity).HasDefaultValue(0).IsRequired();

                entity.Property(e => e.ImageRequiresModeration).HasDefaultValue(false).IsRequired();

                entity.Property(e => e.ImageIsHarmful).HasDefaultValue(false).IsRequired();

                entity.Property(e => e.ImageHateSeverity).HasDefaultValue(0).IsRequired();

                entity.Property(e => e.ImageSelfHarmSeverity).HasDefaultValue(0).IsRequired();

                entity.Property(e => e.ImageSexualSeverity).HasDefaultValue(0).IsRequired();

                entity.Property(e => e.ImageViolenceSeverity).HasDefaultValue(0).IsRequired();

                entity.Property(e => e.CreatedAt).IsRequired();

                entity.Property(e => e.LastUpdatedAt).IsRequired(false);
            });
        }
    }
}
