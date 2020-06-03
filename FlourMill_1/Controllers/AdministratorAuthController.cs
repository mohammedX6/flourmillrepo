using DatingApp.Data;
using DatingApp.Dtos;
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
    public class AdministratorAuthController : ControllerBase
    {
        private readonly IDataRepository _repo;
        private readonly IConfiguration _config;
        private readonly DataContext _context;

        public AdministratorAuthController(IDataRepository repo, IConfiguration config, DataContext context)
        {
            _repo = repo;
            _config = config;
            _context = context;
        }

        [HttpGet]
        [Route("get_all")]
        public IActionResult getAllFlouMills()
        {
            var td = (from od in _context.Administrator

                      select new
                      {
                          od.JobNumber,

                          od.Username,
                          od.Email
                          ,
                          od.Id
                      }).ToList();
            return Ok(td);
        }

        [HttpPost("admin_register")]
        public async Task<IActionResult> AdminRegister(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if (await _repo.AdminExists(userForRegisterDto.Username))
            {
                return BadRequest("User alerady regaistred Admin");
            }

            var AdminCreation = new Administrator
            {
                Username = userForRegisterDto.Username,
                Email = userForRegisterDto.Email,
                BirthDate = userForRegisterDto.BirthDate,
                JobNumber = userForRegisterDto.JobNumber,
                NationalId = userForRegisterDto.NationalId,
                PhoneNumber = userForRegisterDto.PhoneNumber,
                TotalFlourMillPayment = userForRegisterDto.TotalFlourMillPayment
            };

            var createdUser = await _repo.AdminReg(AdminCreation, userForRegisterDto.Password);

            var getUser = (from pd in _context.Administrator
                           where pd.Username == userForRegisterDto.Username
                           select new
                           {
                               pd.Id,
                               pd.Username,
                               pd.Email
                           }).FirstOrDefault();

            var userFromRepo = await _repo.AdminLogin(userForRegisterDto.Username.ToLower(), userForRegisterDto.Password);

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
                uInfo = getUser
            });
        }

        [HttpPost("admin_register_facebook")]
        public async Task<IActionResult> AdminRegisterFacebook(RegisterDTOFacebook userForRegisterDto)
        {
            if(userForRegisterDto.Email==null)
            {
                userForRegisterDto.Email = userForRegisterDto.Username + "@gmail.com";
            }
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if (await _repo.AdminExists(userForRegisterDto.Username))
            {
                var userFromRepo = await _repo.AdminLoginFacebook(userForRegisterDto.Username.ToLower());

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

                var getUser2 = (from pd in _context.Administrator
                                where pd.Username == userForRegisterDto.Username
                                select new
                                {
                                    pd.Id,
                                    pd.Username,
                                    pd.Email
                                }).FirstOrDefault();

                return Ok(new
                {
                    uInfo = getUser2,
                    token = tokenHandler.WriteToken(token)
                });
            }

            var AdminCreation = new Administrator
            {
                Username = userForRegisterDto.Username,
                Email = userForRegisterDto.Email,
                BirthDate = userForRegisterDto.BirthDate,
                JobNumber = userForRegisterDto.JobNumber,
                NationalId = userForRegisterDto.NationalId,
                PhoneNumber = userForRegisterDto.PhoneNumber,
                TotalFlourMillPayment = userForRegisterDto.TotalFlourMillPayment
            };

            var createdUser = await _repo.AdminRegFacebook(AdminCreation);

            var getUser = (from pd in _context.Administrator
                           where pd.Username == userForRegisterDto.Username
                           select new
                           {
                               pd.Id,
                               pd.Username,
                               pd.Email
                           }).FirstOrDefault();

            var userFromRepo2 = await _repo.AdminLoginFacebook(userForRegisterDto.Username.ToLower());

            if (userFromRepo2 == null)
            {
                return Unauthorized();
            }
            var clamis2 = new[]
            {
                new Claim (ClaimTypes.NameIdentifier,userFromRepo2.Id.ToString()),
                new Claim (ClaimTypes.Name,userFromRepo2.Username)
            };

            var key2 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds2 = new SigningCredentials(key2, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor2 = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(clamis2),
                Expires = DateTime.Now.AddDays(365),
                SigningCredentials = creds2
            };
            var tokenHandler2 = new JwtSecurityTokenHandler();

            var token2 = tokenHandler2.CreateToken(tokenDescriptor2);

            return Ok(new
            {
                uInfo = getUser,
                token = tokenHandler2.WriteToken(token2)
            });
        }

        [HttpPost("admin_login")]
        public async Task<IActionResult> AdminLogin(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.AdminLogin(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

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
        [Route("checkadmin/{name}")]
        public async Task<IActionResult> checkFacebook(String name)
        {
            name = name.Trim();
            name = name.ToLower();

            var c = _context.Administrator.FirstOrDefault(x => x.Username == name);
            if (c == null)
            {
                return Ok(new { check = "no" });
            }
            else
            {
                var userFromRepo = await _repo.AdminLoginFacebook(c.Username);

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
                    check = "yes"
                });
            }
        }

        [HttpPost]
        [Route("init_all")]
        public async Task<IActionResult> InitiateSuperVisorANDTruckDriver()
        {
            string name = "moh";
            string password = "moh123";
            if (await _repo.SuperVisorExists(name))
            {
                return BadRequest("SuperVisor alerady initiaed");
            }

            var SuperVisorCreation = new SuperVisor
            {
                Username = "moh",
                Email = "moh@gmail.com",
                BirthDate = "10/10/1998",
                JobNumber = "9874455599",
                NationalId = 9987445555,
            };

            var createdUser = await _repo.SuperVisorReg(SuperVisorCreation, password);

            string name3 = "moh";
            string password3 = "moh123";

            if (await _repo.TruckDriverExists(name3))
            {
                return BadRequest("Admin alerady initiaed");
            }

            var AdminCreateion = new Administrator
            {
                Username = "moh",
                Email = "moh@gmail.com",
                BirthDate = "1/1/1998",
                JobNumber = "989989595",
                NationalId = 9959595959,
                PhoneNumber = "7777777777",
            };

            var createdUser3 = await _repo.AdminReg(AdminCreateion, password3);

            string name2 = "moh";
            string password2 = "moh123";

            if (await _repo.TruckDriverExists(name2))
            {
                return BadRequest("TruckDriver alerady initiaed");
            }

            var TruckDriverCreation = new TruckDriver
            {
                Id = "1",
                Username = "moh",
                Email = "moh@gmail.com",
                BirthDate = "1/1/1998",
                JobNumber = "989989595",
                NationalId = 9959595959,
                PhoneNumber = "7777777777",
                AdministratorID = 1
            };

            var createdUser2 = await _repo.TruckDriverReg(TruckDriverCreation, password2);

            return Ok(new { createdUser = "Supervisor Initiated ", createdUser2 = "Truck driver Initiated", createdUser3 = "Admin Created" });
        }
    }
}