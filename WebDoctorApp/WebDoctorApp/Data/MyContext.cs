using Microsoft.EntityFrameworkCore;
using WebDoctorApp.Models;

namespace WebDoctorApp.Data;

public class MyContext : DbContext
{
    protected MyContext()
    {
    }

    public MyContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}