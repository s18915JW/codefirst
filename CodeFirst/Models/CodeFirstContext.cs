using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

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

            Seed(modelBuilder);
        }

        protected void Seed(ModelBuilder modelBuilder)
        {
            var doctors = new List<Doctor>
            {
                new Doctor { IdDoctor = 1, FirstName = "Jan", LastName = "Jankowski", Email = "jj@mail.com" },
                new Doctor { IdDoctor = 2, FirstName = "Maciej", LastName = "Maciejewski", Email = "mm@mail.com" }
            };
            modelBuilder.Entity<Doctor>().HasData(doctors);

            var patients = new List<Patient>
            {
                new Patient { IdPatient = 1, FirstName = "Bożydar", LastName = "Bożydarowicz", Birthdate = DateTime.Parse("1970-01-01")},
                new Patient { IdPatient = 2, FirstName = "Stanisław", LastName = "Stanisławowicz", Birthdate = DateTime.Parse("1980-02-02")},
            };
            modelBuilder.Entity<Patient>().HasData(patients);

            var meds = new List<Medicament>
            {
                new Medicament { IdMedicament = 1, Name = "Ibuprom", Description = "Działanie przeciwbólowe",  Type = "Przeciwzapalny"},
                new Medicament { IdMedicament = 2, Name = "Zyrtec", Description = "Działanie przeciwzapalnie",  Type = "Przeciwalergiczny"},
            };
            modelBuilder.Entity<Medicament>().HasData(meds);

            var prescriptions = new List<Prescription>
            {
                new Prescription { IdPrescription = 1, Date = DateTime.Parse("2010-01-01"), DueDate = DateTime.Parse("2010-02-02"), IdPatient = 1, IdDoctor = 1},
                new Prescription { IdPrescription = 2, Date = DateTime.Parse("2011-03-03"), DueDate = DateTime.Parse("2011-04-04"), IdPatient = 2, IdDoctor = 2},
            };
            modelBuilder.Entity<Prescription>().HasData(prescriptions);

            var pres_meds = new List<PrescriptionMedicament>()
            {
                new PrescriptionMedicament { IdMedicament = 1, IdPrescription = 1, Dose = 200, Details = "1/dzień"},
                new PrescriptionMedicament { IdMedicament = 2, IdPrescription = 2, Dose = 20, Details = "1/dzień"},
            };
            modelBuilder.Entity<PrescriptionMedicament>().HasData(pres_meds);
        }

    }

}