using System;
using System.Threading.Tasks;
using Template.Services.AccountService.Models;

namespace Template.Services.AccountService
{
    public interface IAccountService
    {
        Task AddAsync(Account account, string password);

        Task<Account> FindAsync(string login);
        Task DeleteAsync(string login);
    }
}
