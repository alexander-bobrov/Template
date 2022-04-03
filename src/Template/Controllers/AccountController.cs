using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Template.Models.Requests;
using Template.Services.AccountService;
using Template.Services.AccountService.Models;

namespace Template.Controllers
{
    [ApiController]
    [Authorize]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly AccountService accountService;

        public AccountController(ILogger<AccountController> logger, AccountService accountService)
        {
            this.logger = logger;
            this.accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] AccountRequest request)
        {
            var accountExists = await accountService.FindAsync(request.Login) is not null;
            if (accountExists) return Conflict();

            var accountId = accountService.AddAsync(new Account { Login = request.Login }, request.Password);

            return Ok(accountId);
        }
        
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            //todo validation
            await accountService.DeleteAsync(id);
            return NoContent();
        }
    }
}