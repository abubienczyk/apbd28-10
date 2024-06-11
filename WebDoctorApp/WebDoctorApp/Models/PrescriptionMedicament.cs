using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace WebDoctorApp.Models;
[PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]

[Table("PrescriptionMedicament")]
public class PrescriptionMedicament
{
    public int IdMedicament { get; set; }
    public int IdPrescription { get; set; }
    public int? Dose { get; set; }
    [MaxLength(100)]
    public string Details { get; set; }= string.Empty;

    [ForeignKey(nameof(IdMedicament))] 
    public Medicament Medicament { get; set; } = null!;
    [ForeignKey(nameof(IdPrescription))]
    public Prescription Prescription { get; set; } = null!;
}