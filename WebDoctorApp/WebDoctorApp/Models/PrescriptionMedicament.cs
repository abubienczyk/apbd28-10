using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace WebDoctorApp.Models;
[PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]

[Table("PrescriptionMedicament")]
public class PrescriptionMedicament
{
    [ForeignKey(nameof(IdMedicament))]
    public int IdMedicament { get; set; }
    [ForeignKey(nameof(IdPrescription))]
    public int IdPrescription { get; set; }
    public int Dose { get; set; }
    [MaxLength(100)]
    public string Details { get; set; }
}