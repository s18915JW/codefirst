using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Models
{
    public class CodeFirstContext : DbContext
    {
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Prescription> Prescription { get; set; }
        public DbSet<Medicament> Medicament { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicament { get; set; }

        public CodeFirstContext(DbContextOptions<CodeFirstContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.IdPatient).HasName("Patient_pk");
                // entity.Property(e => e.IdPatient).ValueGeneratedNever();    

                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();

                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();

                entity.Property(e => e.Birthdate).HasColumnType("date").IsRequired();
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.IdDoctor).HasName("Doctor_pk");
                // entity.Property(e => e.IdPatient).ValueGeneratedNever();    

                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();

                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();

                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(e => e.IdPrescription).HasName("Prescription_pk");
                // entity.Property(e => e.IdPrescription).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date").IsRequired();

                entity.Property(e => e.DueDate).HasColumnType("date").IsRequired();

                entity.HasOne(p => p.Patient)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(p => p.IdPatient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Prescription_Patient");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(d => d.IdDoctor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Prescription_Doctor");
            });

            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.HasKey(e => e.IdMedicament).HasName("Medicament_pk");
                // entity.Property(e => e.IdPatient).ValueGeneratedNever();    

                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();

                entity.Property(e => e.Description).HasMaxLength(100).IsRequired();

                entity.Property(e => e.Type).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<PrescriptionMedicament>(entity =>
            {
                entity.ToTable("Prescription_Medicament");

                entity.HasKey(e => new {e.IdPrescription, e.IdMedicament});

                entity.Property(e => e.Dose).IsRequired();

                entity.Property(e => e.Details).HasMaxLength(100).IsRequired();

                entity.HasOne(p => p.Prescription)
                    .WithMany(p => p.PrescriptionMedicaments)
                    .HasForeignKey(p => p.IdPrescription)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PrescriptionMedicament_Prescription");

                entity.HasOne(m => m.Medicament)
                    .WithMany(p => p.PrescriptionMedicaments)
                    .HasForeignKey(m => m.IdMedicament)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PrescriptionMedicament_Medicament");
            });
        }

    }

}