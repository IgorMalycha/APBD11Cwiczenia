using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APBD9_pracadomowa.DTOs;
using APBD9_pracadomowa.Helpers;
using APBD9_pracadomowa.Models;
using APBD9_pracadomowa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace APBD9_pracadomowa.Controllers;

[Route("user")]
[ApiController]
public class UserController : ControllerBase
{
    private DbServices _dbServices;

    public UserController(DbServices dbServices)
    {
        _dbServices = dbServices;
    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterRequestDTO registerRequestDto)
    {
        var hashedPasswordAndSalt = SecurityHelper.GetHashedPasswordAndSalt(registerRequestDto.Password);

        var user = new AppUser()
        {
            Login = registerRequestDto.Login,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelper.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1)
        };
        
        _dbServices.AddUser(user);
        
        return Created();
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginRequestDTO loginRequestDto)
    {
        AppUser user = await _dbServices.GetUser(loginRequestDto.Login);

        string passwordHashFromDb = user.Password;
        string curHashedPassword = SecurityHelper.GetHashedPasswordWithSalt(loginRequestDto.Password, user.Salt);

        if (passwordHashFromDb != curHashedPassword)
        {
            return Unauthorized();
        }


        Claim[] userclaim = new[]
        {
            new Claim(ClaimTypes.Name, "jakis"),
            new Claim(ClaimTypes.Role, "user"),
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tajnyklucz"));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: userclaim,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelper.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        //_context.SaveChanges();

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = user.RefreshToken
        });
    }
    
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequestDTO refreshToken)
    {
        AppUser user = await _dbServices.GetUserByRefreshToken(refreshToken);
        if (user == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        if (user.RefreshTokenExp < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }
        
        Claim[] userclaim = new[]
        {
            new Claim(ClaimTypes.Name, "pgago"),
            new Claim(ClaimTypes.Role, "user"),
            new Claim(ClaimTypes.Role, "admin")
            //Add additional data here
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tajnyklucz"));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtToken = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: userclaim,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelper.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        //_context.SaveChanges();

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            refreshToken = user.RefreshToken
        });
    }
    
    
}