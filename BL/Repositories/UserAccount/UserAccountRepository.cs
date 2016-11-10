using System;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories.UserAccount
{
    public class UserAccountRepository : EntityFrameworkRepository<Riganti.Utils.Infrastructure.EntityFramework.UserAccount, Guid>
    {
        public UserAccountRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
