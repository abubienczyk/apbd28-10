using Microsoft.EntityFrameworkCore;
using WebDoctorApp.Data;
using WebDoctorApp.DTO_s;
using WebDoctorApp.Models;

namespace WebDoctorApp.Services;

public class DbService : IDbService
{
    private readonly Context _context;

    public DbService(Context context)
    {
        _context = context;
    }

    public async  Task<bool> DoesMedicamentExists(int id)
    {
        return await _context.Medicaments.AnyAsync(e => e.IdMedicament==id);
    }

    public async Task<bool> DoesPatientExists(int id)
    {
        return await _context.Patients.AnyAsync(e => e.IdPatient == id);
    }

    public async Task InsertNewPatient(PatientDTO dto)
    {
       await _context.Patients.AddAsync(new Patient()
        {
            IdPatient = dto.IdPatient,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Birthday = dto.Birthday
        });
        await _context.SaveChangesAsync();
    }

    public async Task AddNewPrescription(Prescription p)
    {
        await _context.AddAsync(p);
        await _context.SaveChangesAsync();

    }

    public async Task AddNewPMData(IEnumerable<PrescriptionMedicament> data)
    {
        await _context.AddRangeAsync(data);
        await _context.SaveChangesAsync();
    }
    
    // public async Task<ICollection<Order>> GetOrdersData(string? clientLastName)
    // {
    //     return await _context.Orders
    //         .Include(e => e.Client)
    //         .Include(e => e.OrderPastries)
    //         .ThenInclude(e => e.Pastry)
    //         .Where(e => clientLastName == null || e.Client.LastName == clientLastName)
    //         .ToListAsync();
    // }
    
    public async Task<ICollection<Prescription>> GetPatientData(int id)
    {
        return await _context.Prescriptions
            .Include(e => e.Patient)
            .Include(e => e.PrescriptionMedicaments)
            .ThenInclude(e => e.Medicament)
            .Include(p => p.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Prescription)
            .ThenInclude(p => p.Doctor)
            .Where(p => p.Patient.IdPatient == id)
            .ToListAsync();
    }
}