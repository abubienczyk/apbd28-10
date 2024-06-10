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
}