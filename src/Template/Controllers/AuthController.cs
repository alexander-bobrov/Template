using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Template.Configuration;
using Template.Models.Requests;

namespace Template.Controllers
{
    [ApiController]
    [Route("token")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly TemplateContext _db;
        private readonly SecurityOptions _securityOptions;

        public AuthController(ILogger<AuthController> logger, TemplateContext db, IOptions<SecurityOptions> securityOptions)
        {
            _logger = logger;
            _db = db;
            _securityOptions = securityOptions.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Authorize([FromBody] AccountRequest request)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(x => x.Login == request.Login);
            if (account is null) return NotFound("Account not found");
            
            var passwordVerification = new PasswordHasher<object>().VerifyHashedPassword(null, account.Login, request.Password);
            if (passwordVerification == PasswordVerificationResult.Failed) return Unauthorized("Invalid password"); 
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityOptions.SecretKey));    
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials));
            
            return Ok(token);
        }
    }
}