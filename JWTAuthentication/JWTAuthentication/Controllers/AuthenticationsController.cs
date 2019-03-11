using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        // GET api/ Authentications Test Authentication
        [HttpGet, Authorize]
        public IActionResult Get()
        {
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(User.Identities.FirstOrDefault().Name));
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            if (user.UserName == "johndoe" && user.Password == "def@123")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "Manager")
                };

                var tokeOptions = new JwtSecurityToken(
                    issuer: "Issuer",
                    audience: "Audience",
                    claims: claims,
                    //notBefore: DateTime.Now,
                    expires: DateTime.Now.AddMinutes(1),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
