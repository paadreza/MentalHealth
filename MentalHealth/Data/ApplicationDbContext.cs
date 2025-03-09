using MentalHealth.Models;
using Microsoft.EntityFrameworkCore;

namespace MentalHealth.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MentalHealthSymptoms> Symptoms { get; set; }
        public DbSet<TrainingDataModel> TrainingData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MentalHealthSymptoms>()
                .HasIndex(s => s.CreatedAt);

            modelBuilder.Entity<TrainingDataModel>()
                .HasIndex(t => t.ImportDate);
        }
    }
}
