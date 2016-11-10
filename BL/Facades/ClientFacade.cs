using BL.DTOs.Clients;
using BL.DTOs.Filters;
using BL.DTOs.Songlists;
using BL.Services.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Facades
{
    public class ClientFacade
    {
        public int PageSize => clientService.ClientPageSize;

        private readonly IClientService clientService;

        public ClientFacade(IClientService clientService)
        {
            this.clientService = clientService;
        }

        /// <summary>
        /// Gets client (including its user account) according to email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Client with specified email</returns>
        public ClientDTO GetClientAccordingToEmail(string email)
        {
            return clientService.GetClientByEmail(email);
        }

        /// <summary>
        /// Gets all clients according to page
        /// </summary>
        /// <param name="requiredPage">page to display</param>
        /// <returns>all clients</returns>
        public ClientListQueryResultDTO GetClients(int requiredPage = 1)
        {
            return clientService.ListAllClients(requiredPage);
        }

        /// <summary>
        /// Gets all songlists according to filter and required page
        /// </summary>
        /// <param name="filter">songlist filter</param>
        /// <param name="requiredPage">page to show</param>
        /// <returns>All songlists</returns>
        public IEnumerable<SonglistDTO> GetSonglists(SonglistFilter filter, int requiredPage = 1)
        {
            return clientService.GetSonglistsForClient(filter, requiredPage, requiredPage);
        }
    }
}
