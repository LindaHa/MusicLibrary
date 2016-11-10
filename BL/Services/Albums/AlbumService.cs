using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.AlbumReviews;
using BL.DTOs.Albums;
using BL.DTOs.Filters;
using BL.Queries;
using BL.Repositories;
using AutoMapper;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using BL.DTOs.Artists;
using BL.DTOs.Songs;
using BL.DTOs.Clients;
using BL.Repositories.UserAccount;

namespace BL.Services.Albums
{
    public class AlbumService : MusicLibraryService, IAlbumService
    {
        public int PageSize => 9;

        private AlbumListQuery albumListQuery;
        private AlbumRepository albumRepository;
        private AlbumReviewListQuery albumReviewListQuery;
        private AlbumReviewRepository albumReviewRepository;
        private ArtistRepository artistRepository;
        private SongListQuery songListQuery;
        private SongRepository songRepository;
        private ArtistListQuery artistListQuery;
        private ClientRepository clientRepository;
        public AlbumService(AlbumRepository albumRepository, AlbumListQuery albumListQuery,AlbumReviewListQuery albumReviewListQuery, 
            AlbumReviewRepository albumReviewRepository, ArtistRepository artistRepository, SongListQuery songListQuery, 
            SongRepository songRepository, ArtistListQuery artistListQuery, ClientRepository clientRepository)
        {
            this.albumRepository = albumRepository;
            this.albumListQuery = albumListQuery;
            this.albumReviewRepository = albumReviewRepository;
            this.albumReviewListQuery = albumReviewListQuery;
            this.artistRepository = artistRepository;
            this.songListQuery = songListQuery;
            this.songRepository = songRepository;
            this.artistListQuery = artistListQuery;
            this.clientRepository = clientRepository;
        }
        
        public void CreateAlbum(AlbumDTO albumDTO)
        {
            if (albumDTO == null)
                throw new ArgumentNullException("Album service - CreateAlbum(...) albumDTO cannot be null");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var album = Mapper.Map<Album>(albumDTO);
                album.Artist = GetAlbumArtist(albumDTO.ArtistID);

                Song song;
                AlbumReview review;
                foreach (int songID in albumDTO.SongIDs)
                {
                    song = GetAlbumSong(songID);
                    album.Songs.Add(song);
                }
                foreach (int albumID in albumDTO.ReviewIDs)
                {
                    review = GetAlbumReview(albumID);
                    album.Reviews.Add(review);
                }

                album.Creator = GetAlbumCreator(albumDTO.CreatorID);

                albumRepository.Insert(album);
                uow.Commit();
            }
        }

        public void DeleteAlbum(int albumId)
        {
            if (albumId < 1)
                throw new ArgumentOutOfRangeException("Album service - DeleteAlbum(...) albumId cannot be lesser than 1");

            using (var uow = UnitOfWorkProvider.Create())
            {
                albumRepository.Delete(albumId);
                uow.Commit();
            }
        }

        public void EditAlbum(AlbumDTO albumDTO, int artistId, int[] albumReviewIds, int[] songIDs)
        {
            if (albumDTO == null)
                throw new ArgumentNullException("Album service - EditAlbum(...) albumDTO cannot be null");
            if (artistId < 1)
                throw new ArgumentOutOfRangeException("Album service - EditAlbum(...) artistId cannot be lesser than 1");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var album = albumRepository.GetByID(albumDTO.ID);
                Mapper.Map(albumDTO, album);

                album.Artist = GetAlbumArtist(artistId);

                if (albumReviewIds != null && albumReviewIds.Any())
                {
                    var albumReviews = albumReviewRepository.GetByIDs(albumReviewIds);
                    album.Reviews.RemoveAll(review => !albumReviews.Contains(review));
                    album.Reviews.AddRange(
                        albumReviews.Where(review => !album.Reviews.Contains(review)));
                }
                else
                {
                    album.Reviews.Clear();
                }

                if (songIDs != null && songIDs.Any())
                {
                    var albumSongs = songRepository.GetByIDs(songIDs);
                    album.Songs.RemoveAll(song => !albumSongs.Contains(song));
                    album.Songs.AddRange(
                        albumSongs.Where(song => !album.Songs.Contains(song)));
                }
                else
                {
                    album.Songs.Clear();
                }

                albumRepository.Update(album);
                uow.Commit();
            }
        }

        public AlbumDTO GetAlbum(int albumId)
        {
            if (albumId < 1)
                throw new ArgumentOutOfRangeException("Album service - GetAlbum(...) albumId cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var album = albumRepository.GetByID(albumId);

                if (album == null)
                    throw new NullReferenceException("Album service - EditAlbum(...) album cannot be null(could not be found)");

                return album != null ? Mapper.Map<AlbumDTO>(album) : null;
            }
        }

        public int GetAlbumIdByName(string albumName)
        {
            if (albumName == null)
                throw new ArgumentNullException("Album service - GetAlbumIdByName(...) albumName cannot be null");

            using (UnitOfWorkProvider.Create())
            {
                albumListQuery.Filter = new AlbumFilter { Name = albumName };
                var album = albumListQuery.Execute().SingleOrDefault();

                if (album == null)
                    throw new NullReferenceException("Album service - GetAlbumIdByName(...) album cannot be null(could not be found)");

                return album?.ID ?? 0;
            }
        } 

        public IEnumerable<AlbumDTO> ListAllAlbums()
        {
            using (UnitOfWorkProvider.Create())
            {
                albumListQuery.Filter = null;
                return albumListQuery.Execute() ?? new List<AlbumDTO>();
            }
        }

        public AlbumListQueryResultDTO ListAllAlbums(AlbumFilter filter, int requiredPage = 1)
        {           
            using (UnitOfWorkProvider.Create())
            {
                var query = GetQuery(filter);
                query.Skip = Math.Max(0, requiredPage - 1) * PageSize;
                query.Take = PageSize;

                var sortOrder = filter.SortAscending ? SortDirection.Ascending : SortDirection.Descending;

                return new AlbumListQueryResultDTO
                {
                    RequestedPage = requiredPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultsPage = query.Execute(),
                    Filter = filter
                };
            }
        }

        public IEnumerable<AlbumReviewDTO> GetAllAlbumReviews(int albumId)
        {
            if (albumId < 1)
                throw new ArgumentOutOfRangeException("Album service - GetAllAlbumReviews(...) albumId cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                albumReviewListQuery.Filter = new AlbumReviewFilter { AlbumID = albumId };
                return albumReviewListQuery.Execute();
            }
        }

        public IEnumerable<SongDTO> GetAllAlbumSongs(int albumId)
        {
            if (albumId < 1)
                throw new ArgumentOutOfRangeException("Album service - GetAllAlbumSongs(...) albumId cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                songListQuery.Filter = new SongFilter { AlbumID = albumId };
                return songListQuery.Execute();
            }
        }

        
        public ArtistDTO GetArtistOfAlbum(int albumId)
        {
            if (albumId < 1)
                throw new ArgumentOutOfRangeException("Album service - GetArtistOfAlbum(...) albumId cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                return Mapper.Map<ArtistDTO>(albumRepository.GetByID(albumId).Artist);
            }
        }

        /// <summary>
        /// Adds the album
        /// </summary>
        /// <param name="albumDTO">Album details</param>
        public void AddAlbum(AlbumDTO albumDTO)
        {
            if (albumDTO == null)
                throw new ArgumentOutOfRangeException("Album service - AddAlbum(...) albumDTO cannot be null");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var album = Mapper.Map<Album>(albumDTO);
                album.Artist = GetAlbumArtist(albumDTO.ArtistID);
                albumRepository.Insert(album);
                uow.Commit();
            }
        }

        public ClientDTO GetCreator(int albumID)
        {
            if (albumID < 1)
                throw new ArgumentOutOfRangeException("Album service - GetCreator(...) albumID cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var album = albumRepository.GetByID(albumID);
                if (album == null)
                {
                    throw new NullReferenceException("Album service - GetCreator(...) album cannot be null(could not be found)");
                }
                return Mapper.Map<ClientDTO>(album.Creator);
            }
        }

        /// <summary>
        /// Gets the artist for a corresponding album according to the ID
        /// </summary>
        /// <param name="artistId">The artist id</param>
        /// <returns>artist according to ID</returns>
        private Artist GetAlbumArtist(int artistId)
        {
            if (artistId < 1)
                throw new ArgumentOutOfRangeException("Album service - GetAlbumArtist(...) artistId cannot be lesser than 1");

            var artist = artistRepository.GetByID(artistId);
            if (artist == null)
            {
                throw new NullReferenceException("Album service - GetAlbumArtistService(...) artist cant be null");
            }
            return artist;
        }

        /// <summary>
        /// Gets the song for a corresponding album according to the ID
        /// </summary>
        /// <param name="songId">The song id</param>
        /// <returns>song according to ID</returns>
        private Song GetAlbumSong(int songId)
        {
            if (songId < 1)
                throw new ArgumentOutOfRangeException("Album service - GetAlbumSong(...) songId cannot be lesser than 1");

            var song = songRepository.GetByID(songId);
            if (song == null)
            {
                throw new NullReferenceException("Album service - GetAlbumSongService(...) song cant be null");
            }
            return song;
        }

        /// <summary>
        /// Gets the albumReview according to the ID
        /// </summary>
        /// <param name="albumReviewId">The album Review id</param>
        /// <returns>album Review according to ID</returns>
        private AlbumReview GetAlbumReview(int albumReviewId)
        {
            if (albumReviewId < 1)
                throw new ArgumentOutOfRangeException("Album service - GetAlbumReview(...) albumReviewId cannot be lesser than 1");

            var albumReview = albumReviewRepository.GetByID(albumReviewId);
            if (albumReview == null)
            {
                throw new NullReferenceException("Album service - GetAlbumReview (...) album Review cant be null");
            }
            return albumReview;
        }

        /// <summary>
        /// Configures albums list query
        /// </summary>
        /// <param name="filter">album filter</param>
        /// <returns>configured query</returns>
        private IQuery<AlbumDTO> GetQuery(AlbumFilter filter)
        {
            var query = albumListQuery;
            query.ClearSortCriterias();
            query.Filter = filter;
            return query;
        }

        /// <summary>
        /// Gets a creator according to the ID
        /// </summary>
        /// <param name="creatorID">The creator ID</param>
        /// <returns>creator according to ID</returns>
        private Client GetAlbumCreator(int creatorID)
        {
            if (creatorID < 1)
                throw new ArgumentOutOfRangeException("Album service - GetAlbumCreator(...) creatorID cannot be lesser than 1");

            var creator = clientRepository.GetByID(creatorID);
            if (creator == null)
            {
                throw new NullReferenceException("Album service - GetAlbumCreator(...) creator cannot be null");
            }
            return creator;
        }
    }
}
