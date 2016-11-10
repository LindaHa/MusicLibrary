using AutoMapper;
using BL.DTOs.AlbumReviews;
using BL.DTOs.Filters;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.Albums;
using Riganti.Utils.Infrastructure.Core;
using BL.DTOs.Clients;
using BL.Repositories.UserAccount;

namespace BL.Services.AlbumReviews
{
    public class AlbumReviewService : MusicLibraryService, IAlbumReviewService
    {
        public int PageSize => 9;

        private AlbumReviewListQuery albumReviewListQuery;
        private AlbumReviewRepository albumReviewRepository;
        private AlbumRepository albumRepository;
        private ClientRepository clientRepository;

        public AlbumReviewService(AlbumReviewListQuery albumReviewListQuery, AlbumReviewRepository albumReviewRepository, 
            AlbumRepository albumRepository, ClientRepository clientRepository)
        {
            this.albumReviewListQuery = albumReviewListQuery;
            this.albumReviewRepository = albumReviewRepository;
            this.albumRepository = albumRepository;
            this.clientRepository = clientRepository;
        }

        /// <summary>
        /// Adds a review
        /// </summary>
        /// <param name="reviewDTO">Review details</param>
        public void AddReview(AlbumReviewDTO reviewDTO)
        {
            if (reviewDTO == null)
                throw new ArgumentNullException("AlbumReview service - AddReview(...) reviewDTO cannot be null");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var review = Mapper.Map<AlbumReview>(reviewDTO);
                review.Album = GetReviewAlbum(reviewDTO.AlbumID);
                albumReviewRepository.Insert(review);
                uow.Commit();
            }
        }

        public AlbumReviewDTO GetAlbumReview(int albumReviewId)
        {
            if (albumReviewId < 1)
                throw new ArgumentOutOfRangeException("AlbumReview service - GetAlbumReview(...) albumReviewId cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var albumReview = albumReviewRepository.GetByID(albumReviewId);
                return albumReview != null ? Mapper.Map<AlbumReviewDTO>(albumReview) : null;
            }
        }

        public void CreateAlbumReview(AlbumReviewDTO albumReviewDTO)
        {
            if (albumReviewDTO == null)
            {
                throw new ArgumentNullException("Album review service - CreateReview(...) review cannot be null");
            }
            using (var uow = UnitOfWorkProvider.Create())
            {
                var albumReview = Mapper.Map<AlbumReviewDTO, AlbumReview>(albumReviewDTO);
                albumReview.Album = GetReviewAlbum(albumReview.Album.ID);

                if (albumReview == null)
                {
                    throw new NullReferenceException("Album review service - CreateReview(...) albumReview cannot be null(could not be found)");
                }

                albumReview.Creator = GetAlbumReviewCreator(albumReviewDTO.CreatorID);

                albumReviewRepository.Insert(albumReview);
                uow.Commit();
            }
        }

        public AlbumReviewListQueryResultDTO ListAllAlbumReviews(AlbumReviewFilter filter, int requiredPage = 1)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = GetQuery(filter);
                query.Skip = Math.Max(0, requiredPage - 1) * PageSize;
                query.Take = PageSize;

                var sortOrder = filter.SortAscending ? SortDirection.Ascending : SortDirection.Descending;
                query.AddSortCriteria(albumReview => albumReview.ID, sortOrder);
                return new AlbumReviewListQueryResultDTO
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
        public void EditAlbumReview(AlbumReviewDTO reviewDTO)
        {
            if (reviewDTO == null)
            {
                throw new ArgumentNullException("Album review service - EditAlbumReview(...) reviewDTO cannot be null");
            }
            using (var uow = UnitOfWorkProvider.Create())
            {
                var review = albumReviewRepository.GetByID(reviewDTO.ID, albumReview => albumReview.Album);
                if (review == null)
                {
                    throw new NullReferenceException("Album review service - EditReview(...) the review cannot be null");
                }
                Mapper.Map(reviewDTO, review);
                albumReviewRepository.Update(review);
                uow.Commit();
            }
        }

        /// <summary>
        /// Delets the review
        /// </summary>
        /// <param name="reviewID">ID of review to delete</param>
        public void DeleteAlbumReview(int reviewID)
        {
            if (reviewID < 1)
                throw new ArgumentOutOfRangeException("AlbumReview service - DeleteAlbumReview(...) reviewID cannot be lesser than 1");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var review = albumReviewRepository.GetByID(reviewID);
                if (review == null)
                {
                    throw new NullReferenceException("Album review service - DeleteAlbumReview(...) review to be deleted is null(could not be found)");
                }
                albumReviewRepository.Delete(review);
                uow.Commit();
            }
        }

        public ClientDTO GetCreator(int albumReviewID)
        {
            if (albumReviewID < 1)
                throw new ArgumentOutOfRangeException("AlbumReview service - GetCreator(...) albumReviewID cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var review = albumReviewRepository.GetByID(albumReviewID);
                if (review == null)
                {
                    throw new NullReferenceException("Album review service - GetCreator(...) review cannot be null");
                }
                return Mapper.Map<ClientDTO>(review.Creator);
            }
        }

        /// <summary>
        /// Gets the album for a corresponding review according to the ID
        /// </summary>
        /// <param name="albumId">The album id</param>
        /// <returns>album according to ID</returns>
        private Album GetReviewAlbum(int albumId)
        {
            if (albumId < 1)
                throw new ArgumentOutOfRangeException("AlbumReview service - GetReviewAlbum(...) albumId cannot be lesser than 1");

            var album = albumRepository.GetByID(albumId);
            if (album == null)
            {
                throw new NullReferenceException("Album review service - GetReviewAlbum(...) the album is null (album could not be found)");
            }
            return album;
        }

        /// <summary>
        /// Configures products list query
        /// </summary>
        /// <param name="filter">product filter</param>
        /// <returns>configured query</returns>
        private IQuery<AlbumReviewDTO> GetQuery(AlbumReviewFilter filter, int requiredPage = 1)
        {
            var query = albumReviewListQuery;
            query.Skip = Math.Max(0, requiredPage - 1) * PageSize;
            query.Take = PageSize;
            query.ClearSortCriterias();
            query.Filter = filter;
            return query;
        }

        /// <summary>
        /// Gets a creator according to the ID
        /// </summary>
        /// <param name="creatorID">The creator ID</param>
        /// <returns>creator according to ID</returns>
        private Client GetAlbumReviewCreator(int creatorID)
        {
            if (creatorID < 1)
                throw new ArgumentOutOfRangeException("AlbumReview service - GetAlbumReviewCreator(...) creatorID cannot be lesser than 1");

            var creator = clientRepository.GetByID(creatorID);
            if (creator == null)
            {
                throw new NullReferenceException("AlbumReview service - GetAlbumReviewCreator(...) creator cannot be null");
            }
            return creator;
        }
    }
}
