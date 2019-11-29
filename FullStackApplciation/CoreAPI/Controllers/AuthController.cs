using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataLayerAbstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelsLibrary.DataModels;
using ModelsLibrary.DTOS;

namespace CoreAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _config;

        #region Constructor

        public AuthController(IAuthRepository repository, IConfiguration config )
        {
            _repository = repository;
            _config = config;
        }

        #endregion


        #region ActionMethods

        [HttpGet("getdata")]
        public async Task<IActionResult> Get()
        {

            return Ok("Test");
        }


        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="userForRegisterDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserForRegisterDto userForRegisterDto)
        {
            var username = userForRegisterDto.Username.ToLower();            
            if (await _repository.PersonExists(username))
            {
                return BadRequest("Username already exists");
            }            

            var person = new Person
            {
                Id = Guid.NewGuid(),
                Username = username                
            };

            var user = await _repository.Register(person, userForRegisterDto.Password);

            if (user == null)
            {
                return BadRequest("Failed to register user");
            }

            return Ok(user);
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserForLoginDto userForLoginDto)
        {
            var user = await _repository.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Tokens:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor { 
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }

        #endregion


    }
}