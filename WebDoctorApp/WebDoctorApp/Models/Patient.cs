using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDoctorApp.Models;

[Table("Patient")]
public class Patient
{
    [Key]
    public int IdPatient { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; }
    [MaxLength(100)]
    public string LastName { get; set; }
    public DateOnly Birthday { get; set; }
    public ICollection<Prescription> Prescriptions { get; set; }
}