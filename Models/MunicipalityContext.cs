using Microsoft.EntityFrameworkCore;

namespace MunicipalityManagementSystem.Models
{
    public class MunicipalityContext : DbContext
    {
        public MunicipalityContext(DbContextOptions<MunicipalityContext> options)
            : base(options)
        {
        }

        // DbSets for the entities
        public DbSet<Citizen> Citizens { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Report> Reports { get; set; }

        // Configure the models using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define primary key for ServiceRequest (explicitly)
            modelBuilder.Entity<ServiceRequest>()
                .HasKey(sr => sr.RequestID);

            // Other primary key configurations
            modelBuilder.Entity<Citizen>()
                .HasKey(c => c.CitizenID); // Citizen entity primary key

            modelBuilder.Entity<Staff>()
                .HasKey(s => s.StaffID); // Staff entity primary key

            modelBuilder.Entity<Report>()
                .HasKey(r => r.ReportID); // Report entity primary key

            // Define relationships
            modelBuilder.Entity<Report>()
               .HasOne(r => r.Citizen) // Navigation property in Report
               .WithMany() // A Citizen can have many Reports
               .HasForeignKey(r => r.CitizenID)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Citizen) // Navigation property in ServiceRequest
                .WithMany() // A Citizen can have many ServiceRequests
                .HasForeignKey(sr => sr.CitizenID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
