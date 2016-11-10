using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.SongReviews;
using BL.Queries;
using BL.Repositories;
using BL.DTOs.Filters;
using AutoMapper;
using DAL.Entities;
using BL.DTOs.Songs;
using Riganti.Utils.Infrastructure.Core;
using BL.DTOs.Clients;
using BL.Repositories.UserAccount;

namespace BL.Services.SongReviews
{
    public class SongReviewService : MusicLibraryService, ISongReviewService
    {
        public int ProductsPageSize => 9;

        private SongReviewListQuery songReviewListQuery;

        private SongReviewRepository songReviewRepository;

        private SongRepository songRepository;

        private ClientRepository clientRepository;

        public SongReviewService(SongReviewListQuery songReviewListQuery, SongReviewRepository songReviewRepository, 
            SongRepository songRepository, ClientRepository clientRepository)
        {
            this.songReviewListQuery = songReviewListQuery;
            this.songReviewRepository = songReviewRepository;
            this.songRepository = songRepository;
            this.clientRepository = clientRepository;
        }
        
        /// <summary>
        /// Adds a review
        /// </summary>
        /// <param name="reviewDTO">Review details</param>
        public void AddReview(SongReviewDTO reviewDTO)
        {
            if (reviewDTO == null)
                throw new NullReferenceException("SongReview Service - AddReview(...) reviewDTO cannot be null");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var review = Mapper.Map<SongReview>(reviewDTO);
                review.Song = GetReviewSong(reviewDTO.SongID);
                songReviewRepository.Insert(review);
                uow.Commit();
            }
        }

        public SongReviewDTO GetSongReview(int songReviewId)
        {
            if (songReviewId < 1)
                throw new ArgumentOutOfRangeException("SongReview Service - GetSongReview(...) songReviewId cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var songReview = songReviewRepository.GetByID(songReviewId);
                return songReview != null ? Mapper.Map<SongReviewDTO>(songReview) : null;
            }
        }

        public void CreateSongReview(SongReviewDTO songReviewDTO)
        {
            if (songReviewDTO == null)
                throw new NullReferenceException("SongReview Service - CreateSongReview(...) songReviewDTO cannot be null");

            if (songReviewDTO == null)
            {
                throw new NullReferenceException("Song review service - CreateReview(...) review cannot be null");
            }
            using (var uow = UnitOfWorkProvider.Create())
            {
                var songReview = Mapper.Map<SongReviewDTO, SongReview>(songReviewDTO);
                songReview.Song = GetReviewSong(songReview.Song.ID);
                songReview.Creator = GetSongReviewCreator(songReviewDTO.CreatorID);


                songReviewRepository.Insert(songReview);
                uow.Commit();
            }
        }        

        public SongReviewListQueryResultDTO ListAllSongReviews(SongReviewFilter filter, int requiredPage = 1)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = GetQuery(filter);
                query.Skip = Math.Max(0, requiredPage - 1) * ProductsPageSize;
                query.Take = ProductsPageSize;

                var sortOrder = filter.SortAscending ? SortDirection.Ascending : SortDirection.Descending;
                query.AddSortCriteria(review => review.ID);
                return new SongReviewListQueryResultDTO
                {
                    RequestedPage = requiredPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultsPage = query.Execute(),
                    Filter = filter
                };
            }
        }

        /// <summary>
        /// Updates review
        /// </summary>
        /// <param name="reviewDTO">Update details</param>
        public void EditSongReview(SongReviewDTO reviewDTO)
        {
            if (reviewDTO == null)
                throw new ArgumentOutOfRangeException("SongReview Service - EditSongReview(...) reviewDTO cannot be null");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var review = songReviewRepository.GetByID(reviewDTO.ID, songReview => songReview.Song);
                if (review == null)
                {
                    throw new NullReferenceException("Song review service - EditReview(...) the review cannot be null");
                }
                Mapper.Map(reviewDTO, review);
                songReviewRepository.Update(review);
                uow.Commit();
            }
        }

        /// <summary>
        /// Deletes the review
        /// </summary>
        /// <param name="reviewID">ID of review to delete</param>
        public void DeleteSongReview(int reviewID)
        {
            if(reviewID < 1)
                throw new ArgumentOutOfRangeException("Song review service - DeleteReview(...) the reviewID cannot be lesser than 1");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var review = songReviewRepository.GetByID(reviewID);
                if (review == null)
                {
                    throw new NullReferenceException("Song review service - DeleteReview(...) the review to be deleted is null");
                }
                songReviewRepository.Delete(review);
                uow.Commit();
            }
        }
        

        public ClientDTO GetCreator(int songReviewID)
        {
            if (songReviewID < 1)
                throw new ArgumentOutOfRangeException("Song review service - GetCreator(...) the songReviewID cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var songReview = songReviewRepository.GetByID(songReviewID);
                if (songReview == null)
                {
                    throw new NullReferenceException("SongReview service - GetCreator(...) songReview cannot be null");
                }
                return Mapper.Map<ClientDTO>(songReview.Creator);
            }
        }

        /// <summary>
        /// Gets the song for a corresponding review according to the ID
        /// </summary>
        /// <param name="songId">The song id</param>
        /// <returns>song according to ID</returns>
        private Song GetReviewSong(int songId)
        {
            if (songId < 1)
                throw new ArgumentOutOfRangeException("Song review service - GetReviewSong(...) the songId cannot be lesser than 1");

            var song = songRepository.GetByID(songId);
            if (song == null)
            {
                throw new NullReferenceException("Song review service - GetReviewSong(...) the song is null");
            }
            return song;
        }

        /// <summary>
        /// Configures songReviews list query
        /// </summary>
        /// <param name="filter">songReview filter</param>
        /// <returns>configured query</returns>
        private IQuery<SongReviewDTO> GetQuery(SongReviewFilter filter)
        {
            var query = songReviewListQuery;
            query.ClearSortCriterias();
            query.Filter = filter;
            return query;
        }

        /// <summary>
        /// Gets a creator according to the ID
        /// </summary>
        /// <param name="creatorID">The creator ID</param>
        /// <returns>creator according to ID</returns>
        private Client GetSongReviewCreator(int creatorID)
        {
            if (creatorID < 1)
                throw new ArgumentOutOfRangeException("SongReview service - GetSongReviewCreator(...) creatorID cannot be lesser than 1");

            var creator = clientRepository.GetByID(creatorID);
            if (creator == null)
            {
                throw new NullReferenceException("SongReview service - GetSongReviewCreator(...) creator cannot be null");
            }
            return creator;
        }
    }
}
