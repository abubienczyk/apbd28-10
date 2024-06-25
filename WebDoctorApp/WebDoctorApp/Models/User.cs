using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDoctorApp.Models;

[Table("users")]
public class User
{
    [Key]
    public int Id { get; set; }
    public string Login { get; set; } 
    public string Password { get; set; }
    public string Salt { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExp { get; set; }
}