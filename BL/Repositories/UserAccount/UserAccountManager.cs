using System;
using System.Data.Entity;
using System.Linq;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using DAL;

namespace BL.Repositories.UserAccount
{
    /// <summary>
    /// User Account Manager, required by UserAccountService
    /// </summary>
    public class UserAccountManager : DbContextUserAccountRepository<MusicLibraryDbContext, Riganti.Utils.Infrastructure.EntityFramework.UserAccount>, IUserAccountRepository<Riganti.Utils.Infrastructure.EntityFramework.UserAccount>
    {
        public UserAccountManager(Func<DbContext> dbContextFactory)
            : base(dbContextFactory.Invoke() as MusicLibraryDbContext) { }

        protected override IQueryable<Riganti.Utils.Infrastructure.EntityFramework.UserAccount> DefaultQueryFilter(IQueryable<Riganti.Utils.Infrastructure.EntityFramework.UserAccount> query, string claimFilter)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (claimFilter == null)
            {
                throw new ArgumentNullException(nameof(claimFilter));
            }

            return query.SelectMany(account => account.ClaimCollection, (account, claims) => new { account, claims })
                    .Where(t => t.claims.Value.Contains(claimFilter))
                    .Select(t => t.account);
        }
    }
}
