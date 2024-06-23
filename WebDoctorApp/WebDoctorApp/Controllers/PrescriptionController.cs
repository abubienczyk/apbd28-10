using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebDoctorApp.DTO_s;
using WebDoctorApp.Models;
using WebDoctorApp.Services;

namespace WebDoctorApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionController : ControllerBase
{
    private readonly IDbService _dbService;

    public PrescriptionController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [Authorize]
    [HttpPost("prescriptions")]
    public async Task<IActionResult> AddNewPrescription(AddNewPrescriptionDTO dto)
    {
        if (!await _dbService.DoesPatientExists(dto.patient.IdPatient))
        {
            await _dbService.InsertNewPatient(dto.patient);
        }

        foreach (var med in dto.medicaments)
        {
            if (!await _dbService.DoesMedicamentExists(med.IdMedicament))
                return BadRequest("MEDICAMENT NOT FOUND");
        }

        if (dto.medicaments.Count > 10)
            return BadRequest("TOO MANY MEDICAMENTS");
        if (dto.DueDate < dto.Date)
            return BadRequest("BAD DATE");

        var prescription = new Prescription()
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            IdPatient = dto.patient.IdPatient,
            IdDoctor = dto.doctor.IdDoctor
        };
        var pm = new List<PrescriptionMedicament>();
        foreach (var tmp in dto.medicaments)
        {
            pm.Add(new PrescriptionMedicament()
            {
                IdMedicament = tmp.IdMedicament,
                Details = tmp.Description,
                Prescription = prescription
            });
        }

        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _dbService.AddNewPrescription(prescription);
            await _dbService.AddNewPMData(pm);

            scope.Complete();
        }

        return Created("api/prescriptions",new
        {
            Id=prescription.IdPrescription,
            prescription.Date,
            prescription.DueDate
        });
    }
}