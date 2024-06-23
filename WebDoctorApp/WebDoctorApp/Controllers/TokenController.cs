using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebDoctorApp.Services;

namespace WebDoctorApp.Controllers;

[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    

    private readonly IDbService _dbService;
    private readonly IConfiguration _config;

    public TokenController(IConfiguration config, IDbService dbService)
    {
        _config = config;
        _dbService = dbService;
    }

    [HttpGet("hash-password-without-salt/{password}")]
    public IActionResult HashPassword(string password)
    {
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            new byte[] {0},
            10,
            HashAlgorithmName.SHA512,
            128
        );
    
        return Ok(Convert.ToHexString(hash));
    }
    // hashowanie z sola
    [HttpGet("hash-password/{password}")]
    public IActionResult HashPasswordWithSalt(string password)
    {
        var passwordHasher = new PasswordHasher<User>();
        return Ok(passwordHasher.HashPassword(new User(), password));
    }
    //weryfikacja
    [HttpPost("verify-password")]
    public IActionResult VerifyPassword(VerifyPasswordRequestModel requestModel)
    {
        var passwordHasher = new PasswordHasher<User>();
        return Ok(passwordHasher.VerifyHashedPassword(new User(), requestModel.Hash, requestModel.Password) == PasswordVerificationResult.Success);
    }
    
    //rejestracja nowaego uzytkowanika
    [HttpPost("register")]
    public async Task<IActionResult> Register(LoginRequestModel model)
    {
        var user = new User { Name = model.UserName, Password = model.Password};
        
        try
        {
            await _dbService.AddNewUser(user);
            return Ok(new { Message = "User registered successfully!" });
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Exception during user registration: {ex.Message}");
            return StatusCode(500, new { Message = "Failed to register user." });
        }
     
    }
    
    
    //login z zapisaniem tokena 
    [HttpPost("login")]
    public async Task< IActionResult >Login(LoginRequestModel model) 
    {
        //if(!(model.UserName.ToLower() == "test" && model.Password == "test"))
         if(! await _dbService.GetUser(model.UserName, model.Password))   
        {
            return Unauthorized("Wrong username or password");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescription = new SecurityTokenDescriptor()
        {
            Issuer = _config["JWT:Issuer"],
            Audience = _config["JWT:Audience"],
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!)),
                SecurityAlgorithms.HmacSha256
            )
        };
        var token = tokenHandler.CreateToken(tokenDescription);
        var stringToken = tokenHandler.WriteToken(token);

        var refTokenDescription = new SecurityTokenDescriptor
        {
            Issuer = _config["JWT:RefIssuer"],
            Audience = _config["JWT:RefAudience"],
            Expires = DateTime.UtcNow.AddDays(3),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:RefKey"]!)),
                SecurityAlgorithms.HmacSha256
            )
        };
        var refToken = tokenHandler.CreateToken(refTokenDescription);
        var stringRefToken = tokenHandler.WriteToken(refToken);
        await _dbService.UpdateToken(model.UserName, model.Password, stringRefToken);
        return Ok(new LoginResponseModel
        {
            Token = stringToken,
            RefreshToken = stringRefToken
        });
    }
    
    
    //refresh token --> nowy access token 
    [HttpPost("refresh")]
    public IActionResult RefreshToken(RefreshTokenRequestModel requestModel)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(requestModel.RefreshToken, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["JWT:RefIssuer"],
                ValidAudience = _config["JWT:RefAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:RefKey"]!))
            }, out SecurityToken validatedToken);
            return Ok(true + " " + validatedToken);
        }
        catch
        {
            return Unauthorized();
        }
    }
}

public class User
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Token { get; set; }
}

public class VerifyPasswordRequestModel
{
    public string Password { get; set; } = null!;
    public string Hash { get; set; } = null!;
}

public class LoginRequestModel
{
    [Required]
    public string UserName { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}

public class LoginResponseModel
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}

public class RefreshTokenRequestModel
{
    public string RefreshToken { get; set; } = null!;
}
