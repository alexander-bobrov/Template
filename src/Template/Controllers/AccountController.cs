using System;
using System.Threading.Tasks;
using Database;
using Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Template.Models.Requests;


namespace Template.Controllers
{
    [ApiController]
    [Authorize]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly TemplateContext _db;

        public AccountController(ILogger<AccountController> logger, TemplateContext db)
        {
            _logger = logger;
            _db = db;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] AccountRequest request)
        {
            var accountExists = await _db.Accounts.AnyAsync(x => x.Login == request.Login);
            if (accountExists) return Conflict();
            
            var hashedPassword = new PasswordHasher<object>().HashPassword(null, request.Password);
            var entry = _db.Accounts.Add(new AccountEntity {Login = request.Login, Password = hashedPassword});
            await _db.SaveChangesAsync();

            return Ok(entry.Entity.Id);
        }
        
        [HttpDelete("{id:guid}/delete")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var account = await _db.Accounts.FindAsync(id);
            if (account is null) return NotFound("Account not found");
       
            _db.Accounts.Remove(account);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}