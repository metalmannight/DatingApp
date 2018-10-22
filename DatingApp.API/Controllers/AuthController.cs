using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Members
        private readonly IAuthRepository _repo;
        #endregion Members
        public IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this._config = config;
            this._repo = repo;

        }

        #region Public Methods
        [HttpPost("Register")]
        // public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // validate request
            // if (!ModelState.IsValid)
            //     return BadRequest(ModelState)


            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await this._repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await this._repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
            //return CreatedAtRoute()
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            // Check if we have a user with the correct password
            var userFromRepo = await this._repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            // If no valid user + identification is found, reject the login
            if (userFromRepo == null)
                return Unauthorized();

            // Create the JWT (Json Web Token) Claims part
            // 1. Create the claims for the token, one for the Id and one for the Username
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            // 2. Set hashed token key.
            // In order tp be assured that the token is valid when it comes back later frome the server.
            var key = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(this._config.GetSection("AppSettings:Token").Value));

            // 3. Generate the signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // 4. First part of the actual Token creation.
            // Create the token descriptor (with the lenght of time, etc.)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // 5. Create the token handler, which handles the tokendescription.
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 6. Return the token through the "Ok" status code.
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
            // return StatusCode(100);
        }

        #endregion Public Methods


    }
}