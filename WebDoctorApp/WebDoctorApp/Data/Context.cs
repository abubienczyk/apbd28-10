using Microsoft.EntityFrameworkCore;
using WebDoctorApp.Models;

namespace WebDoctorApp.Data;

public class Context : DbContext
{
    protected Context()
    {
    }

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }
    
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>
        {
             new Doctor { IdDoctor = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jan.kowalski@example.com" },
             new Doctor { IdDoctor = 2, FirstName = "Anna", LastName = "Nowak", Email = "anna.nowak@example.com" }
             
        });
        modelBuilder.Entity<Patient>().HasData(new List<Patient>
        {
            new Patient { IdPatient = 1, FirstName = "Kornel", LastName = "Kowalski",Birthday = new DateOnly(2000, 5, 15) },
            new Patient { IdPatient = 2, FirstName = "Pola", LastName = "Policja",Birthday = new DateOnly(1990, 5, 15)}
             
        });
        modelBuilder.Entity<Medicament>().HasData(new List<Medicament>
        {
            new Medicament
            {
                IdMedicament = 1,
                Name = "Paracetamol",
                Description = "Lek przeciwbólowy i przeciwgorączkowy",
                Type = "Tabletka",
            },
            new Medicament
            {
                IdMedicament = 2,
                Name = "Ibuprofen",
                Description = "Lek przeciwzapalny, przeciwbólowy i przeciwgorączkowy",
                Type = "Kapsułka",
            }
        });
        modelBuilder.Entity<Prescription>().HasData(new List<Prescription>
        {
            new Prescription
            {
            IdPrescription = 1,
            Date = new DateOnly(2024, 6, 10),
            DueDate = new DateOnly(2024, 6, 17),
            IdPatient = 1,
            IdDoctor = 1
        },
        
        new Prescription
        {
            IdPrescription = 2,
            Date = new DateOnly(2024, 6, 11),
            DueDate = new DateOnly(2024, 6, 18),
            IdPatient = 2,
            IdDoctor = 2
        }
        });
        modelBuilder.Entity<PrescriptionMedicament>().HasData(new List<PrescriptionMedicament>
        {
            new PrescriptionMedicament
            {
                IdMedicament = 1,
                IdPrescription = 1,
                Dose = 2,
                Details = "Twice a day after meal"
            }
        });
    }
}