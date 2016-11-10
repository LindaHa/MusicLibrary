using BL.AppInfrastructure;
using BL.DTOs.Clients;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BL.Queries
{ 
    public class ClientAccordingToEmailQuery : AppQuery<ClientDTO>
    {
        public ClientAccordingToEmailQuery(IUnitOfWorkProvider provider) : base(provider) { }

        public string Email { get; set; }

        protected override IQueryable<ClientDTO> GetQueryable()
        {
            if (string.IsNullOrEmpty(Email) || !new EmailAddressAttribute().IsValid(Email))
            {
                throw new InvalidOperationException("ClientAccordingToUserIdQuery - Email must be valid.");
            }

            // Single result is expected so client side execution is not a problem
            Client client = Context.Clients
                .Include(nameof(Client.Account)).FirstOrDefault(c => c.Account.Email.Equals(Email));

            if (client == null)
            {
                return new EnumerableQuery<ClientDTO>(new List<ClientDTO>());
            }

           
            var clientDTO = AutoMapper.Mapper.Map<ClientDTO>(client);

            return new EnumerableQuery<ClientDTO>(new List<ClientDTO> { clientDTO });
        }
    }
}
