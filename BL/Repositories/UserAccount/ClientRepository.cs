using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.UserAccount
{
    public class ClientRepository : EntityFrameworkRepository<Client, int>
    {
        public ClientRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
