using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDoctorApp.Models;

[Table("Patient")]
public class Patient
{
    [Key]
    public int IdPatient { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; }= string.Empty;
    [MaxLength(100)]
    public string LastName { get; set; }= string.Empty;
    public DateTime Birthday { get; set; }
    public ICollection<Prescription> Prescriptions { get; set; } = new HashSet<Prescription>();
}