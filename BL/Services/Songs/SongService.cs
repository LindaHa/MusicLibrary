using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.Songs;
using BL.DTOs.Filters;
using DAL.Entities;
using BL.Queries;
using BL.Repositories;
using AutoMapper;
using Riganti.Utils.Infrastructure.Core;
using BL.DTOs.SongReviews;
using BL.DTOs.Albums;
using BL.DTOs.Clients;
using BL.Repositories.UserAccount;

namespace BL.Services.Songs
{
    public class SongService : MusicLibraryService, ISongService
    {
        public int PageSize => 9;

        private SongListQuery songListQuery;
        private SongRepository songRepository;
        private SongReviewListQuery songReviewListQuery;
        private SongReviewRepository songReviewRepository;
        private AlbumRepository albumRepository;
        private ClientRepository clientRepository;

        public SongService(SongRepository songRepository, SongListQuery songListQuery, SongReviewListQuery songReviewListQuery, 
            SongReviewRepository songReviewRepository, AlbumRepository albumRepository, ClientRepository clientRepository)
        {
            this.songRepository = songRepository;
            this.songListQuery = songListQuery;
            this.songReviewRepository = songReviewRepository;
            this.songReviewListQuery = songReviewListQuery;
            this.albumRepository = albumRepository;
            this.clientRepository = clientRepository;
        }

        public void CreateSong(SongDTO songDTO)
        {
            if (songDTO == null)
                throw new ArgumentNullException("Song service - CreateSong(...) songDTO cannot be null");

            SongReview review;
            var song = Mapper.Map<Song>(songDTO);

            using (var uow = UnitOfWorkProvider.Create())
            {
                song.Album = GetSongAlbum(songDTO.AlbumID);
                song.Creator = clientRepository.GetByID(songDTO.CreatorID);
                if (songDTO.ReviewIDs != null)
                {
                    foreach (int ID in songDTO.ReviewIDs)
                    {
                        review = GetSongReview(ID);
                        song.Reviews.Add(review);
                    }
                }

                songRepository.Insert(song);
                uow.Commit();
            }
        }

        public void DeleteSong(int songId)
        {
            if (songId < 1)
                throw new ArgumentOutOfRangeException("Song service - DeleteSong(...) songId cannot be lesser than 1");

            using (var uow = UnitOfWorkProvider.Create())
            {
                songRepository.Delete(songId);
                uow.Commit();
            }
        }

        public void EditSong(SongDTO songDTO, int albumId, List<int> songReviewIds)
        {
            if (songDTO == null)
                throw new ArgumentNullException("Song service - EditSong(...) songDTO cannot be null");
            if (albumId < 1)
                throw new ArgumentOutOfRangeException("Song service - EditSong(...) albumId cannot be lesser than 1");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var song = songRepository.GetByID(songDTO.ID);
                if (song == null)
                    throw new NullReferenceException("Song service - EditSong(...) song cannot be null (no song found)");
                Mapper.Map(songDTO, song);

                song.Album = GetSongAlbum(albumId);

                if (songReviewIds != null && songReviewIds.Any())
                {
                    var songReviews = songReviewRepository.GetByIDs(songReviewIds);
                    song.Reviews.RemoveAll(review => !songReviews.Contains(review));
                    song.Reviews.AddRange(songReviews.Where(review => !song.Reviews.Contains(review)));
                }
                else
                {
                    song.Reviews.Clear();
                }

                songRepository.Update(song);
                uow.Commit();
            }
        }

        public SongDTO GetSong(int songId)
        {
            if (songId < 1)
                throw new ArgumentOutOfRangeException("Song service - GetSong(...) songId cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var song = songRepository.GetByID(songId);
                return song != null ? Mapper.Map<SongDTO>(song) : null;
            }
        }

        public AlbumDTO GetAlbumOfSong(int songID)
        {
            if (songID < 1)
                throw new ArgumentOutOfRangeException("Song service - GetAlbumOfSong(...) songID cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                return Mapper.Map<AlbumDTO>
                    (songRepository.GetByID(songID).Album);
            }
        }

        public int GetSongIdByName(string songName)
        {
            if (songName == null)
                throw new ArgumentNullException("Song service - GetSongIdByName(...) songName cannot be null");

            using (UnitOfWorkProvider.Create())
            {
                songListQuery.Filter = new SongFilter { Name = songName };
                var song = songListQuery.Execute().SingleOrDefault();
                if(song == null)
                    throw new NullReferenceException("Song service - GetSongIdByName(...) song cannot be null (no song found with the given songName)");

                return song.ID;
            }
        }

        public IEnumerable<SongReviewDTO> GetSongReviews(int songID)
        {
            if (songID < 1)
                throw new ArgumentOutOfRangeException("Song service - GetSongReviews(...) songID cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                songReviewListQuery.Filter = new SongReviewFilter { SongID = songID };
                return songReviewListQuery.Execute();
            }
        }

        public SongListQueryResultDTO ListAllSongs(SongFilter filter, int requiredPage = 1)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = GetQuery(filter);
                query.Skip = Math.Max(0, requiredPage - 1) * PageSize;
                query.Take = PageSize;

                var sortOrder = filter.SortAscending ? SortDirection.Ascending : SortDirection.Descending;

                return new SongListQueryResultDTO
                {
                    RequestedPage = requiredPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultsPage = query.Execute(),
                    Filter = filter
                };
            }
        }

        public IEnumerable<SongDTO> ListAllSongs()
        {
            using (UnitOfWorkProvider.Create())
            {
                songListQuery.Filter = null;
                return songListQuery.Execute() ?? new List<SongDTO>();
            }
        }

        /// <summary>
        /// Adds a song
        /// </summary>
        /// <param name="songDTO">Review details</param>
        public void AddSong(SongDTO songDTO)
        {
            if (songDTO == null)
                throw new ArgumentNullException("Song service - AddSong(...) songDTO cannot be null");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var song = Mapper.Map<Song>(songDTO);
                song.Album = GetSongAlbum(songDTO.AlbumID);
                songRepository.Update(song);
                uow.Commit();
            }
        }

        public ClientDTO GetCreator(int songID)
        {
            if (songID < 1)
                throw new ArgumentOutOfRangeException("Song service - GetCreator(...) songID cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var song = songRepository.GetByID(songID);
                if (song == null)
                {
                    throw new NullReferenceException("Song service - GetCreator(...) song cannt be null");
                }
                return Mapper.Map<ClientDTO>(song.Creator);
            }
        }

        /// <summary>
        /// Configures songs list query
        /// </summary>
        /// <param name="filter">song filter</param>
        /// <returns>configured query</returns>
        private IQuery<SongDTO> GetQuery(SongFilter filter)
        {
            var query = songListQuery;
            query.ClearSortCriterias();
            query.Filter = filter;
            return query;
        }

        /// <summary>
        /// Gets the album according to the ID
        /// </summary>
        /// <param name="albumID">The album IDI</param>
        /// <returns>album according to ID</returns>
        private Album GetSongAlbum(int albumID)
        {
            var album = albumRepository.GetByID(albumID);
            if (album == null)
            {
                throw new NullReferenceException("Song service - GetSongAlbum(...) album cannot be null");
            }
            return album;
        }

        /// <summary>
        /// Gets a review according to the ID
        /// </summary>
        /// <param name="reviewID">The album IDI</param>
        /// <returns>album according to ID</returns>
        private SongReview GetSongReview(int reviewID)
        {
            var review = songReviewRepository.GetByID(reviewID);
            if (review == null)
            {
                throw new NullReferenceException("Song service - GetSongReview(...) review cannot be null");
            }
            return review;
        }

        /// <summary>
        /// Gets a creator according to the ID
        /// </summary>
        /// <param name="creatorID">The creator ID</param>
        /// <returns>creator according to ID</returns>
        private Client GetSongCreator(int creatorID)
        {
            if (creatorID < 1)
                throw new ArgumentOutOfRangeException("Song service - GetSongCreator(...) creatorID cannot be lesser than 1");

            var creator = clientRepository.GetByID(creatorID);
            if (creator == null)
            {
                throw new NullReferenceException("Song service - GetSongCreator(...) creator cannot be null");
            }
            return creator;
        }
    }
}
