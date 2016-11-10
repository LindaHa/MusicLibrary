using BL.DTOs.Clients;
using BL.DTOs.Songlists;
using System.Collections.Generic;
using BL.DTOs.Clients;
using System;
using BL.DTOs.Filters;

namespace BL.Services.Clients
{
    /// <summary>
    /// Handles Client CRUD
    /// </summary>  
    public interface IClientService
    {
        int ClientPageSize { get; }

        /// <summary>
        /// Creates new client
        /// </summary>
        /// <param name="userAccountId">userAccountId</param>
        void CreateClient(Guid userAccountId, ClientDTO clientDTO);

        /// <summary>
        /// Updates Client according to ID
        /// </summary>
        /// <param name="ClientDTO">client details</param>
        /// <param name="songlistIDs">songlist IDs</param>
        void EditClient(ClientDTO clientDTO, params int[] songlistIDs);

        /// <summary>
        /// Removes client according to ID
        /// </summary>
        /// <param name="clientId">client ID</param>
        void DeleteClient(int clientId);

        /// <summary>
        /// Gets client according to ID
        /// </summary>
        /// <param name="clientId">client ID</param>
        /// <returns>The client</returns>
        ClientDTO GetClient(int clientId);

        /// <summary>
        /// Gets all clients
        /// </summary>
        /// <param name="requiredPage">page to show</param>
        /// <returns>all available clients</returns>
        ClientListQueryResultDTO ListAllClients(int requiredPage);

        /// <summary>
        /// Gets client Songlists according to filter and required page
        /// </summary>
        /// <param name="filter">songlist filter</param>
        /// <param name="clientId">client ID</param>
        /// <param name="requiredPage"> page to show</param>
        /// <returns>all available songlists</returns>
        IEnumerable<SonglistDTO> GetSonglistsForClient(SonglistFilter filter, int clientId, int requiredPage);



        /// <summary>
        /// Gets client with given email address
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>Client with given email address</returns>
        ClientDTO GetClientByEmail(string email);
    }
}
