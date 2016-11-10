using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.Clients;
using BL.DTOs.Songlists;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;
using AutoMapper;
using Riganti.Utils.Infrastructure.Core;
using BL.Repositories.UserAccount;
using BL.DTOs.Filters;

namespace BL.Services.Clients
{
    public class ClientService : MusicLibraryService, IClientService
    {
        private const int SonglistsPageSize = 10;
        
        public int ClientPageSize => 20;

        private ClientListQuery clientListQuery;
        private ClientAccordingToEmailQuery clientAccordingToEmailQuery;
        private SonglistListQuery songlistListQuery;
        private ClientRepository clientRepository;
        private SonglistRepository songlistRepository;
        private UserAccountRepository userRepository;


        public ClientService(ClientRepository clientRepository, ClientAccordingToEmailQuery clientAccordingToEmailQuery, 
            ClientListQuery clientListQuery, SonglistListQuery songlistListQuery, SonglistRepository songlistRepository, 
            UserAccountRepository userRepository)
        {
            this.clientRepository = clientRepository;
            this.clientAccordingToEmailQuery = clientAccordingToEmailQuery;
            this.clientListQuery = clientListQuery;
            this.songlistRepository = songlistRepository;
            this.songlistListQuery = songlistListQuery;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Creates new customer (user account must be created first)
        /// </summary>
        /// <param name="userAccountId">Customer user account ID</param>
        public void CreateClient(Guid userAccountId, ClientDTO clientDTO)
        {           
            using (var uow = UnitOfWorkProvider.Create())
            {
                var clientAccount = userRepository.GetByID(userAccountId);
                if (clientAccount == null)
                    throw new NullReferenceException("Client service - CreateClient(...) clientAccount cannot be null(not found)");
                
                var client = Mapper.Map<Client>(clientDTO);
                client.Account = clientAccount;
                clientRepository.Insert(client);
                uow.Commit();
            }
        }        

        //public void CreateClient(ClientDTO clientDTO)
        //{
        //    Songlist songlist;
        //    Client client = Mapper.Map<Client>(clientDTO);
        //    using (var uow = UnitOfWorkProvider.Create())
        //    {
        //        foreach (int ID in clientDTO.SonglistIDs)
        //        {
        //            songlist = GetClientSonglist(ID);
        //            client.Songlists.Add(songlist);
        //        }

        //        clientRepository.Insert(client);
        //        uow.Commit();
        //    }
        //}

        public void DeleteClient(int clientId)
        {
            if (clientId < 1)
                throw new ArgumentOutOfRangeException("Client service - DeleteClient(...)clientId cannot be lesser than 1");

            using (var uow = UnitOfWorkProvider.Create())
            {
                clientRepository.Delete(clientId);
                uow.Commit();
            }
        }

        public void EditClient(ClientDTO clientDTO, List<int> songlistIDs)
        {
            if (clientDTO == null)
                throw new ArgumentNullException("Client service - EditClient(...)clientDTO cannot be null");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var client = clientRepository.GetByID(clientDTO.ID);
                if (client == null)
                    throw new NullReferenceException("Client service - EditClient(...) client cannot be null(not found)");

                Mapper.Map(clientDTO, client);

                if (songlistIDs != null && songlistIDs.Any())
                {
                    var songlists = songlistRepository.GetByIDs(songlistIDs);
                    client.Songlists.RemoveAll(review => !songlists.Contains(review));
                    client.Songlists.AddRange(
                        songlists.Where(review => !client.Songlists.Contains(review)));
                }
                else
                {
                    client.Songlists.Clear();
                }

                clientRepository.Update(client);
                uow.Commit();
            }
        }

        public ClientDTO GetClient(int clientId)
        {
            if (clientId < 1)
                throw new ArgumentOutOfRangeException("Client service - GetClient(...)clientId cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var client = clientRepository.GetByID(clientId);
                return client != null ? Mapper.Map<ClientDTO>(client) : null;
            }
        }

        public ClientListQueryResultDTO ListAllClients(int requiredPage = 1)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = GetClientQuery();
                query.Skip = Math.Max(0, requiredPage - 1) * ClientPageSize;
                query.Take = ClientPageSize;
                query.AddSortCriteria(client => client.LastName);


                return new ClientListQueryResultDTO
                {
                    RequestedPage = requiredPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultsPage = query.Execute(),
                };
            }
        }

        /// <summary>
        /// Gets client with given email address
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>Client with given email address</returns>
        public ClientDTO GetClientByEmail(string email)
        {
            if (email == null)
                throw new ArgumentNullException("Client service - GetClient(...)email cannot be null");

            using (UnitOfWorkProvider.Create())
            {
                clientAccordingToEmailQuery.Email = email;
                return clientAccordingToEmailQuery.Execute().SingleOrDefault();
            }
        }

        public IEnumerable<SonglistDTO> GetSonglistsForClient(SonglistFilter filter, int clientId, int requiredPage = 1)
        {
            if (clientId < 1)
                throw new ArgumentOutOfRangeException("Client service - GetSonglistsForClient(...)clientId cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                songlistListQuery.Filter = filter;
                songlistListQuery.Skip = Math.Max(0, requiredPage - 1) * SonglistsPageSize;
                songlistListQuery.Take = SonglistsPageSize;

                var sortSonglist = filter.SortAscending ? SortDirection.Ascending : SortDirection.Descending;
                songlistListQuery.AddSortCriteria(songlist => songlist.Name, sortSonglist);

                return songlistListQuery.Execute();
            }
        }

        /// <summary>
        /// Configures clients list query
        /// </summary>
        /// <param name="filter">client filter</param>
        /// <returns>configured query</returns>
        private IQuery<ClientDTO> GetClientQuery()
        {
            var query = clientListQuery;
            query.ClearSortCriterias();
            return query;
        }

        /// <summary>
        /// Gets a songlist according to the ID
        /// </summary>
        /// <param name="songlistID">The songlist ID</param>
        /// <returns>songlist according to ID</returns>
        private Songlist GetClientSonglist(int songlistID)
        {
            if (songlistID < 1)
                throw new ArgumentOutOfRangeException("Client service - GetClientSonglist(...)songlistID cannot be lesser than 1");

            var songlist = songlistRepository.GetByID(songlistID);
            if (songlist == null)
            {
                throw new NullReferenceException("Client service - GetClientSonglist(...) songlist cannot be null");
            }
            return songlist;
        }
    }
}
