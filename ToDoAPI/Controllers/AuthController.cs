using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ToDoAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ToDoAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthContext _context;
        private readonly DbSet<UserItem> _users = null;
        private readonly IConfiguration _configuration;

        public AuthController(AuthContext context, IConfiguration configuration)
        {
            //Getting list of usernames from database
            _context = context;
            _users = _context.Users;
            _configuration = configuration;
        }

        [HttpPost("Token")]
        public IActionResult GenerateToken()
        {
            //Read the request header for identifying request type and credentials passed
            var header = Request.Headers["Authorization"];
            //Need to check if it's Basic authorization
            if (header.ToString().StartsWith("Basic"))
            {
                //Read credentials sent by TODO App/client for sending back the token                
                var credentials = header.ToString().Substring("Basic ".Length).Trim();
                var usernamePass = Encoding.UTF8.GetString(Convert.FromBase64String(credentials));
                var userPass = usernamePass.Split(":");
                //check username and password in our sqlite db
                var foundUser = _users.Where(user => user.UserName == userPass[0] && user.Password == userPass[1]).FirstOrDefault();
                //Note: Ideally, we would store the user passwords in encrypted form in the database using a good encryption algorithm
                //and would encrypt the password provided and compare it against the value in the database, instead of storing a plaintext password
                if (foundUser == null)
                {
                    //User not found
                    return Unauthorized();
                }
                //Generate claim for token key
                var claim = new[] { new Claim(ClaimTypes.Name, userPass[0]) };
                //We use JWT authenticaion using a symmetric key algorithm
                //Get key, issuer and audience details from appsettings.json config file
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthenticationSettings:EncryptionKey"]));
                //passing in key and algorithm details
                var signingInCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
                //Pass in issuer, audience, timeout, etc. details
                var securityToken = new JwtSecurityToken(issuer: _configuration["AuthenticationSettings:Issuer"], audience: _configuration["AuthenticationSettings:Audience"], expires: DateTime.Now.AddMinutes(1)
                    , claims: claim, signingCredentials: signingInCredentials);
                //Generate the token
                string generatedToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
                //Return the token with status code 200
                return Ok(generatedToken);
            }
            //If the request is not basic authorization return status code 400 with below message
            return BadRequest("Invalid request type");
        }
    }
}