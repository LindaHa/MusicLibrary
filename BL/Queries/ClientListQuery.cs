using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Clients;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Queries
{
    public class ClientListQuery : AppQuery<ClientDTO>
    {
        public ClientListQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<ClientDTO> GetQueryable()
        {
            return Context.Clients
                .Include(nameof(Client.Songlists))
                .ProjectTo<ClientDTO>();        }

    }
}
