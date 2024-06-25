using Microsoft.EntityFrameworkCore;
using WebDoctorApp.Controllers;
using WebDoctorApp.Data;
using WebDoctorApp.DTO_s;
using WebDoctorApp.Helpers;
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
    
    public async Task<Patient?> GetData(int id)
    {
        var patient= await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == id);
        patient.Prescriptions = patient.Prescriptions.OrderBy(pr => pr.DueDate).ToList();
        return patient;
        
    }

    public async Task<User> GetUser(string name)
    {
         return await _context.Users.FirstOrDefaultAsync(u => u.Login == name );
    }

    public async Task AddNewUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateToken(string name, string password, string token)
    {
        var user= await _context.Users.FirstOrDefaultAsync(u => u.Login == name && u.Password == password);
        user.RefreshToken = token;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task SaveTokenInfo(User user, string tokeRef, DateTime exp)
    {
        user.RefreshToken = SecurityHelper.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        _context.SaveChanges();
    }

    public async Task<User> GetUserByToken(string token)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == token );
    }
}