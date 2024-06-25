using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebDoctorApp.Helpers;
using WebDoctorApp.Models;
using WebDoctorApp.Services;

namespace WebDoctorApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController  : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IDbService _dbService;

    public AuthenticationController(IConfiguration configuration, IDbService dbService)
    {
        _config = configuration;
        _dbService = dbService;
    }


    //rejestracja nowaego uzytkowanika
    [HttpPost("register")]
    public async Task<IActionResult> Register(LoginRequestModel model)
    {
        var hashedPasswordAndSalt = SecurityHelper.GetHashedPasswordAndSalt(model.Password);

        var user = new User()
        {
            Login = model.UserName,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelper.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1)
        };
        
            await _dbService.AddNewUser(user);
            return Ok(new { Message = "User registered successfully!" });
     
    }
    //login z zapisaniem tokena 
    [HttpPost("login")]
    public async Task< IActionResult >Login(LoginRequestModel model)
    {
        User user = await _dbService.GetUser(model.UserName);
        string passwordHashFromDb = user.Password;
        string curHashedPassword = SecurityHelper.GetHashedPasswordWithSalt(model.Password, user.Salt);

        if (passwordHashFromDb != curHashedPassword)
        {
            return Unauthorized();
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
        
        string tokenRef=SecurityHelper.GenerateRefreshToken();

        await _dbService.SaveTokenInfo(user, tokenRef, DateTime.Now.AddDays(1));
        
        return Ok(new LoginResponseModel
        {
            Token = stringToken,
            RefreshToken = tokenRef
        });
        
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestModel model)
    {
        User user = await _dbService.GetUserByToken(model.RefreshToken);
        if (user == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        if (user.RefreshTokenExp < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }
        var tokenHandler = new JwtSecurityTokenHandler();
        
        tokenHandler.ValidateToken(model.RefreshToken, new TokenValidationParameters
        {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["JWT:RefIssuer"],
                ValidAudience = _config["JWT:RefAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:RefKey"]!))
            }, out SecurityToken validatedToken);
        
        string tokenRef=SecurityHelper.GenerateRefreshToken();

        await _dbService.SaveTokenInfo(user, tokenRef, DateTime.Now.AddDays(1));

        return Ok(new
        {
            accessToken =validatedToken, 
            refreshToken = tokenRef
        });
    }
}