using System;

namespace Database.Entities
{
    public class AccountEntity : BaseEntity
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }

    }
}