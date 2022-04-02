using Database;
using Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template.Services.AccountService.Models;

namespace Template.Services.AccountService
{
    public class AccountService
    {
        private TemplateContext db;

        public AccountService(TemplateContext db)
        {
            this.db = db;
        }
        public async Task AddAsync(Account account, string password)
        {
            var hashedPassword = new PasswordHasher<object>().HashPassword(null, password);
            db.Accounts.Add(new AccountEntity { Login = account.Login, PasswordHash = hashedPassword });
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = db.Accounts.FirstOrDefault(x => x.Id == id);
            if (entity == null) return;

            db.Accounts.Remove(entity);
            await db.SaveChangesAsync();
        }

        public async Task<Account> FindAsync(string login)
        {           
            var entity = await db.Accounts.AsNoTracking()
                .Where(x => x.Login == login)
                .FirstOrDefaultAsync();

            return entity is null ? null : new Account();
        }
    }
}