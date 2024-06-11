using Microsoft.AspNetCore.Mvc;
using WebDoctorApp.DTO_s;
using WebDoctorApp.Models;
using WebDoctorApp.Services;

namespace WebDoctorApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly IDbService _dbService;

    public PatientController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet]
    public async Task<IActionResult> GrtPatientData(int id)
    {
        var presciptions = await _dbService.GetPatientData(id);
        return Ok(presciptions.Select(e => new GetPatientDTO()
        {
            patient = new PatientDTO()
            {
                IdPatient = e.IdPatient,
                FirstName = e.Patient.FirstName,
                LastName = e.Patient.LastName,
                Birthday = e.Patient.Birthday
            },
            prescriptions = presciptions.Select(p => new PrescriptionDTO()
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentDTO
                {
                    IdMedicament = pm.IdMedicament,
                    Name = pm.Medicament.Name,
                    Description = pm.Medicament.Description,
                    Type = pm.Medicament.Type
                }).ToList(),
                doctors = p.PrescriptionMedicaments.Select(pm => new DoctorDTO
                {
                    IdDoctor = pm.Prescription.Doctor.IdDoctor,
                    FirstName = pm.Prescription.Doctor.FirstName,
                }).ToList()
            }).ToList()
        }));
    }
}