using FlourMill_1.Data;
using FlourMill_1.Dtos;
using FlourMill_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FlourMill_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BakeryAuthController : ControllerBase
    {
        private readonly IDataRepository _repo;
        private readonly IConfiguration _config;
        private readonly DataContext _context;

        public BakeryAuthController(IDataRepository repo, IConfiguration config, DataContext context)
        {
            _repo = repo;
            _config = config;
            _context = context;
        }

        [HttpPost("bakery_register")]
        public async Task<IActionResult> BakeryRegister(BakeryRegisterDTO userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if (await _repo.BakeryExists(userForRegisterDto.Username))
            {
                return BadRequest("User alerady regaistred Bakery");
            }

            var BakeryCreation = new Bakery
            {
                Username = userForRegisterDto.Username,
                Email = userForRegisterDto.Email,
                BirthDate = userForRegisterDto.BirthDate,
                JobNumber = userForRegisterDto.JobNumber,
                NationalId = userForRegisterDto.NationalId,
                PhoneNumber = userForRegisterDto.PhoneNumber
                ,
                address = userForRegisterDto.address,
                latitude = userForRegisterDto.latitude,
                longitude = userForRegisterDto.longitude
            };

            var createdUser = await _repo.BakeryReg(BakeryCreation, userForRegisterDto.Password);

            var userFromRepo = await _repo.BakeryLogin(userForRegisterDto.Email.ToLower(), userForRegisterDto.Password);

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

            var getUser = (from pd in _context.Bakery
                           where pd.Email == userForRegisterDto.Email
                           select new
                           {
                               pd.Id,
                               pd.Username,
                               pd.Email,
                               pd.BirthDate,
                               pd.JobNumber,
                               pd.NationalId,
                               pd.PhoneNumber,
                               pd.address,
                               pd.latitude,
                               pd.longitude
                           }).FirstOrDefault();

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user = getUser
            });
        }

        [HttpPost("bakery_login")]
        public async Task<IActionResult> BakeryLogin(FirebaseForLoginDTO firebaseForLoginDTO)
        {
            var userFromRepo = await _repo.BakeryLogin(firebaseForLoginDTO.email.ToLower(), firebaseForLoginDTO.Password);
            var getUser = (from pd in _context.Bakery
                           where pd.Email == firebaseForLoginDTO.email
                           select new
                           {
                               pd.Id,
                               pd.Username,
                               pd.Email,
                               pd.BirthDate,
                               pd.JobNumber,
                               pd.NationalId,
                               pd.PhoneNumber,
                               pd.address,
                               pd.latitude,
                               pd.longitude
                           }).FirstOrDefault();

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
                user = getUser
            });
        }
    }
}