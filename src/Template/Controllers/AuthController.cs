using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Template.Configuration;
using Template.Controllers.Requests;
using Template.Services.AccountService;

namespace Template.Controllers
{
    [ApiController]
    [Route("token")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly AccountService accountService;
        private readonly SecurityOptions securityOptions;

        public AuthController(ILogger<AuthController> logger, AccountService accountService, IOptions<SecurityOptions> securityOptions)
        {
            this.logger = logger;
            this.accountService = accountService;
            this.securityOptions = securityOptions.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Authorize([FromBody] AccountRequest request)
        {
            var account = await accountService.FindAsync(request.Login);
            if (account is null) return NotFound("Account not found");
            
            var password = await accountService.GetPasswordAsync(account.Login);
            var passwordVerification = new PasswordHasher<object>().VerifyHashedPassword(null, password, request.Password);
            if (passwordVerification == PasswordVerificationResult.Failed) return Unauthorized("You hasn't been authorized"); 
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityOptions.SecretKey));    
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials));
            
            return Ok(token);
        }
    }
}