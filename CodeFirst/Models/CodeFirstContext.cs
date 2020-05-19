using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Models
{
    public class CodeFirstContext : DbContext
    {
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Prescription> Prescription { get; set; }

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

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(e => e.IdPrescription).HasName("Prescription_pk");
                // entity.Property(e => e.IdPrescription).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date").IsRequired();

                entity.Property(e => e.DueDate).HasColumnType("date").IsRequired();

                entity.HasOne(p => p.Patient)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(p => p.IdPatient)
                    .OnDelete(DeleteBehavior.ClientSetNull)    // sami chcemy usuwać związki od góry
                    .HasConstraintName("Prescription_Patient");


            });

        }

    }

}