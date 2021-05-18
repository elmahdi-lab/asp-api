using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PomeloHealthApi.Models;

namespace PomeloHealthApi.Database
{
    public partial class DatabaseContext : DbContext
    {
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Availability> Availabilities { get; set; }

        public DatabaseContext()
        {

        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Availability>(entity => {
                entity.HasKey(k => k.Id);
                entity.HasOne(c => c.Provider)
                    .WithMany()
                    .IsRequired();
                entity.HasIndex(i => i.StartDate);
                entity.HasIndex(i => i.EndDate);
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.HasOne(c => c.Provider)
                    .WithMany()
                    .IsRequired();
                entity.HasOne(c => c.Patient)
                    .WithMany()
                    .IsRequired();
                entity.HasIndex(i => i.StartDate);
                entity.HasIndex(i => i.EndDate);
            });

            modelBuilder.Entity<Patient>(entity => {
                entity.Property(b => b.Id)
                    .HasComputedColumnSql("CONCAT(Firstname, ' ', Lastname)", stored: true);
                entity.Property(b => b.Id)
                    .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            });

            modelBuilder.Entity<Provider>(entity => {
                entity.Property(b => b.Id)
                    .HasComputedColumnSql("CONCAT(Firstname, ' ', Lastname)", stored: true);
                entity.Property(b => b.Id)
                    .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            });

        }
    }
}