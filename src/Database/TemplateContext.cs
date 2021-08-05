using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public sealed class TemplateContext : DbContext
    {
        public TemplateContext(DbContextOptions<TemplateContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        //dotnet ef migrations add --startup-project Template --project Database Initial
        public DbSet<AccountEntity> Accounts { get; set; }

            
    }
    
    
}