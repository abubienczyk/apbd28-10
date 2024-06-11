using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebDoctorApp.Models;
[Table("Medicament")]
public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }= string.Empty;
    [MaxLength(100)]
    public string Description { get; set; }= string.Empty;
    [MaxLength(100)]
    public string Type { get; set; }= string.Empty;

    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } =
        new HashSet<PrescriptionMedicament>();

}