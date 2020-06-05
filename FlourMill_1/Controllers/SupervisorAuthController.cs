using FlourMill_1.Data;
using FlourMill_1.Dtos;
using FlourMill_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FlourMill_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupervisorAuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IDataRepository _repo;

        public SupervisorAuthController(IDataRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("supervisor_register")]
        public async Task<IActionResult> SuperVisorRegister(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if (await _repo.SuperVisorExists(userForRegisterDto.Username))
            {
                return BadRequest("User alerady regaistred SuperVisor");
            }

            var SuperVisorCreation = new SuperVisor
            {
                Username = userForRegisterDto.Username,
                Email = userForRegisterDto.Email,
                BirthDate = userForRegisterDto.BirthDate,
                JobNumber = userForRegisterDto.JobNumber,
                NationalId = userForRegisterDto.NationalId,
            };

            var createdUser = await _repo.SuperVisorReg(SuperVisorCreation, userForRegisterDto.Password);

            return Ok(new { createdUser = "Supervisor created succefully " });
        }

        [HttpPost("supervisor_login")]
        public async Task<IActionResult> SupervisorLogin(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.SuperVisorLogin(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
            {
                return Unauthorized();
            }
            var clamis = new[]
            {
                new Claim (ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim (ClaimTypes.Name,userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(clamis),
                Expires = DateTime.Now.AddDays(365),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
            }); ;
        }
    }
}