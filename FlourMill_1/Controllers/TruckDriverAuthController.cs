using DatingApp.Data;
using DatingApp.Dtos;
using FlourMill_1.Dtos;
using FlourMill_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class TruckDriverAuthController : ControllerBase
    {
        private readonly IDataRepository _repo;
        private readonly IConfiguration _config;
        private readonly DataContext _context;

        public TruckDriverAuthController(IDataRepository repo, IConfiguration config, DataContext context)
        {
            _repo = repo;
            _config = config;
            _context = context;
        }

        [HttpPost("truckdriver_register")]
        public async Task<IActionResult> TruckDriverRegister(TruckForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if (await _repo.TruckDriverExists(userForRegisterDto.Username))
            {
                return BadRequest("User alerady regaistred TruckDriver");
            }

            var TruckDriverCreation = new TruckDriver
            {
                Id = userForRegisterDto.id,
                Username = userForRegisterDto.Username,
                Email = userForRegisterDto.Email,
                BirthDate = userForRegisterDto.BirthDate,
                JobNumber = userForRegisterDto.JobNumber,
                NationalId = userForRegisterDto.NationalId,
                PhoneNumber = userForRegisterDto.PhoneNumber
                ,
                AdministratorID = 1
            };

            var createdUser = await _repo.TruckDriverReg(TruckDriverCreation, userForRegisterDto.Password);

            var userFromRepo = await _repo.TruckDriverLogin(userForRegisterDto.Email.ToLower(), userForRegisterDto.Password);

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

        [HttpPost("truckdriver_facebookregister")]
        public async Task<IActionResult> TruckDriverRegisterFacebook(RegisterDTOFacebook userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if (await _repo.TruckDriverExists(userForRegisterDto.Username))
            {
                return BadRequest("User alerady regaistred TruckDriver");
            }

            var TruckDriverCreation = new TruckDriver
            {
                Id = userForRegisterDto.id,
                Username = userForRegisterDto.Username,
                Email = userForRegisterDto.Email,
                BirthDate = userForRegisterDto.BirthDate,
                JobNumber = userForRegisterDto.JobNumber,
                NationalId = userForRegisterDto.NationalId,
                PhoneNumber = userForRegisterDto.PhoneNumber,
                AdministratorID = 1
            };

            var createdUser = await _repo.TruckDriverRegFacebook(TruckDriverCreation);

            var userFromRepo = await _repo.TruckDriverLoginFacebook(userForRegisterDto.Email.ToLower());

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
            });
        }

        [HttpPost("truckdriver_login")]
        public async Task<IActionResult> truckdriverlogin(FirebaseForLoginDTO truckForLoginDTO)
        {
            var userFromRepo = await _repo.TruckDriverLogin(truckForLoginDTO.email.ToLower(), truckForLoginDTO.Password);

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
            });
        }

        [HttpGet]
        [Route("checkTruck/{uid}")]
        public async Task<IActionResult> checkFacebook(String uid)
        {
            uid = uid.Trim();

            var c = await _context.TruckDriver.FirstOrDefaultAsync(x => x.Id == uid);

            if (c == null)
            {
 
                return Ok(new { check = "no" });
            }
            else
            {
          
                var userFromRepo = await _repo.TruckDriverLoginFacebook(c.Email);

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
                    check = "yes"
                });
            }
        }
    }
}