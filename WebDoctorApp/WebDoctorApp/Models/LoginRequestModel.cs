using System.ComponentModel.DataAnnotations;

namespace WebDoctorApp.Models;

public class LoginRequestModel
{
    [Required]
    public string UserName { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}