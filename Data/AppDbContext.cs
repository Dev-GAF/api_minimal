using Microsoft.EntityFrameworkCore;
using api_minimal; 

namespace api_minimal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Monitor> Monitores { get; set; }
        public DbSet<Horario> Horarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Config relaçao monitor -> horario
            modelBuilder.Entity<Horario>()
            .HasOne(h => h.Monitor)
            .WithMany(m => m.Horarios)
            .HasForeignKey(h => h.IdMonitor)
            .OnDelete(DeleteBehavior.Cascade);

            // Indices
            modelBuilder.Entity<Monitor>()
            .HasIndex(m => m.RA)
            .IsUnique();

            modelBuilder.Entity<Monitor>()
            .HasIndex(m => m.Apelido)
            .IsUnique();
        }
    }
}