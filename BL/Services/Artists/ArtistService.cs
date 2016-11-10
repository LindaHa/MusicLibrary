using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.Artists;
using BL.Queries;
using BL.Repositories;
using AutoMapper;
using DAL.Entities;
using BL.DTOs.Filters;
using Riganti.Utils.Infrastructure.Core;
using BL.DTOs.Albums;
using BL.DTOs.Clients;
using BL.Repositories.UserAccount;

namespace BL.Services.Artists
{
    public class ArtistService : MusicLibraryService, IArtistService
    {
        public int PageSize => 9;

        private ArtistListQuery artistListQuery;
        private AlbumListQuery albumListQuery;
        private ArtistRepository artistRepository;
        private AlbumRepository albumRepository;
        private ClientRepository clientRepository;

        public ArtistService(ArtistRepository artistRepository, ArtistListQuery artistListQuery, AlbumListQuery albumListQuery, 
            AlbumRepository albumRepository, ClientRepository clientRepository)
        {
            this.artistRepository = artistRepository;
            this.artistListQuery = artistListQuery;
            this.albumRepository = albumRepository;
            this.albumListQuery = albumListQuery;
            this.clientRepository = clientRepository;
        }

        public void CreateArtist(ArtistDTO artistDTO)
        {
            if (artistDTO == null)
                throw new ArgumentNullException("Artist service - CreateArtist(...) artistDTO cannot be null");

            Album album;
            Artist artist = Mapper.Map<Artist>(artistDTO);
            using (var uow = UnitOfWorkProvider.Create())
            {
                if (artistDTO.AlbumIDs != null)
                {
                    foreach (int ID in artistDTO.AlbumIDs)
                    {
                        album = GetArtistAlbum(ID);

                        if (album == null)
                            throw new NullReferenceException("Artist service - CreateArtist(...) album cannot be null(could not be found)");

                        artist.Albums.Add(album);
                    }
                }

                artist.Creator = GetArtistCreator(artistDTO.CreatorID);            

                artistRepository.Insert(artist);
                uow.Commit();
            }
        }

        public void DeleteArtist(int artistId)
        {
            if (artistId < 1)
                throw new ArgumentOutOfRangeException("Artist service - DeleteArtist(...) artistId cannot be lesser than 1");

            using (var uow = UnitOfWorkProvider.Create())
            {
                artistRepository.Delete(artistId);
                uow.Commit();
            }
        }

        public void EditArtist(ArtistDTO artistDTO, List<int> albumIds)
        {
            if (artistDTO == null)
                throw new ArgumentNullException("Artist service - EditArtist(...) artistDTO cannot be null");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var artist = artistRepository.GetByID(artistDTO.ID);
                Mapper.Map(artistDTO, artist);

                if (albumIds != null && albumIds.Any())
                {
                    var albums = albumRepository.GetByIDs(albumIds);
                    if (albums == null)
                        throw new NullReferenceException("Artist service - EditArtist(...) album cannot be null(could not be found)");

                    artist.Albums.RemoveAll(review => !albums.Contains(review));
                    artist.Albums.AddRange(
                        albums.Where(review => !artist.Albums.Contains(review)));
                }
                else
                {
                    artist.Albums.Clear();
                }

                artistRepository.Update(artist);
                uow.Commit();
            }
        }

        public ArtistDTO GetArtist(int artistId)
        {
            if (artistId < 1)
                throw new ArgumentOutOfRangeException("Artist service - GetArtist(...) artistId cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var artist = artistRepository.GetByID(artistId);
                return artist != null ? Mapper.Map<ArtistDTO>(artist) : null;
            }
        }

        public int GetArtistIdByName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("Artist service - GetArtistIdByName(...) name cannot be null");

            using (UnitOfWorkProvider.Create())
            {
                artistListQuery.Filter = new ArtistFilter { Name = name };
                var artist = artistListQuery.Execute().SingleOrDefault();
                return artist?.ID ?? 0;
            }
        }

        public IEnumerable<ArtistDTO> ListAllArtists()
        {
            using (UnitOfWorkProvider.Create())
            {
                artistListQuery.Filter = null;
                return artistListQuery.Execute() ?? new List<ArtistDTO>();
            }
        }

        public ArtistListQueryResultDTO ListAllArtists(ArtistFilter filter, int requiredPage = 1)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = GetQuery(filter);
                query.Skip = Math.Max(0, requiredPage - 1) * PageSize;
                query.Take = PageSize;

                var sortOrder = filter.SortAscending ? SortDirection.Ascending : SortDirection.Descending;
                query.AddSortCriteria(artist => artist.Name, sortOrder);

                return new ArtistListQueryResultDTO
                {
                    RequestedPage = requiredPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultsPage = query.Execute(),
                    Filter = filter
                };
            }
        }


        public IEnumerable<AlbumDTO> GetAllAlbumsForArtist(int artistId)
        {
            if (artistId < 1)
                throw new ArgumentOutOfRangeException("Artist service - GetAllAlbumsForArtist(...) artistId cannot be lesser than 1");

            List<int> artistIds = new List<int> { artistId };
            using (UnitOfWorkProvider.Create())
            {
                albumListQuery.Filter = new AlbumFilter { ArtistIDs = artistIds };
                return albumListQuery.Execute();
            }
        }

        public ClientDTO GetCreator(int artistID)
        {
            if (artistID < 1)
                throw new ArgumentOutOfRangeException("Artist service - GetCreator(...) artistID cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var artist = artistRepository.GetByID(artistID);
                if (artist == null)
                {
                    throw new NullReferenceException("Artist service - GetCreator(...) artist or creator is null");
                }
                return Mapper.Map<ClientDTO>(artist.Creator);
            }
        }

        /// <summary>
        /// Configures artists list query
        /// </summary>
        /// <param name="filter">artist filter</param>
        /// <returns>configured query</returns>
        private IQuery<ArtistDTO> GetQuery(ArtistFilter filter)
        {
            var query = artistListQuery;
            query.ClearSortCriterias();
            query.Filter = filter;
            return query;
        }


        /// <summary>
        /// Gets a album according to the ID
        /// </summary>
        /// <param name="albumID">The album ID</param>
        /// <returns>album according to ID</returns>
        private Album GetArtistAlbum(int albumID)
        {
            if (albumID < 1)
                throw new ArgumentOutOfRangeException("Artist service - GetArtistAlbum(...) albumID cannot be lesser than 1");

            var album = albumRepository.GetByID(albumID);
            if (album == null)
            {
                throw new NullReferenceException("Artist service - GetArtistAlbum(...) album cannot be null");
            }
            return album;
        }

        /// <summary>
        /// Gets a creator according to the ID
        /// </summary>
        /// <param name="creatorID">The creator ID</param>
        /// <returns>creator according to ID</returns>
        private Client GetArtistCreator(int creatorID)
        {
            if (creatorID < 1)
                throw new ArgumentOutOfRangeException("Artist service - GetArtistCreator(...) creatorID cannot be lesser than 1");

            var creator = clientRepository.GetByID(creatorID);
            //if (creator == null)
            //{
            //    throw new NullReferenceException("Artist service - GetArtistAlbum(...) album cannot be null");
            //}
            return creator;
        }
    }
}