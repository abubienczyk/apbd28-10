using WebDoctorApp.Controllers;
using WebDoctorApp.Data;
using WebDoctorApp.DTO_s;
using WebDoctorApp.Models;

namespace WebDoctorApp.Services;

public interface IDbService
{
    public Task<bool> DoesMedicamentExists(int id);
    public Task<bool> DoesPatientExists(int id);
    public Task InsertNewPatient(PatientDTO dto);
    public Task AddNewPrescription(Prescription p);
    public Task AddNewPMData(IEnumerable<PrescriptionMedicament> data);

    //public Task<ICollection<Prescription>> GetPatientData(int id);
    public Task<Patient?> GetData(int id);

    public Task<User> GetUser(string name);
    public Task<User> GetUserByToken(string token);
    public Task AddNewUser(User user);
    public Task SaveTokenInfo(User user,string tokeRef, DateTime exp);
    public Task UpdateToken(string name, string password, string token);
}