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

    public Task<bool> GetUser(string name, string password);
    public Task AddNewUser(User user);
    public Task UpdateToken(string name, string password, string token);
}