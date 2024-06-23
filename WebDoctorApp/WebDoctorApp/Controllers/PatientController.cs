using Microsoft.AspNetCore.Authorization;
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
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPatientData(int id)
    {
        var patient = await _dbService.GetData(id);

        var patientDTO = new GetPatientDTO()
        {
            patient = new PatientDTO()
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Birthday = patient.Birthday
            },
            prescriptions = patient.Prescriptions.Select(p => new PrescriptionDTO()
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                doctor = new DoctorDTO()
                {
                    IdDoctor = p.Doctor.IdDoctor,
                    FirstName = p.Doctor.FirstName
                },
                medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentDTO()
                {
                    IdMedicament = pm.Medicament.IdMedicament,
                    Name = pm.Medicament.Name,
                    Description = pm.Medicament.Description,
                    Type = pm.Medicament.Type,
                }).ToList()
            }).ToList()
        };

        return Ok(patientDTO);
    }

}