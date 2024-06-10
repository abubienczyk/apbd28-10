using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDoctorApp.Models;

[Table("Prescription")]
public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    
    public int IdPatient { get; set; }
    [ForeignKey(nameof(IdPatient))]
    public int IdDoctor { get; set; }
    [ForeignKey(nameof(IdDoctor))]
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}