using BL.DTOs.AlbumReviews;
using BL.DTOs.Clients;
using BL.DTOs.Filters;

namespace BL.Services.AlbumReviews
{   /// <summary>
    /// Handles AlbumReviewReview CRUDDeleteClbumReview
    /// </summary>  
    public interface IAlbumReviewService : ICreator
    {
        int PageSize { get; }

        /// <summary>
        /// Creates new albumReview
        /// </summary>
        /// <param name="albumReviewDTO">albumReview details</param>
        void CreateAlbumReview(AlbumReviewDTO albumReviewDTO);

        /// <summary>
        /// Updates AlbumReview according to ID
        /// </summary>
        /// <param name="AlbumReviewDTO">albumReview details</param>
        void EditAlbumReview(AlbumReviewDTO albumReviewDTO);

        /// <summary>
        /// Removes albumReview according to ID
        /// </summary>
        /// <param name="albumReviewId">albumReview ID</param>
        void DeleteAlbumReview(int albumReviewId);

        /// <summary>
        /// Gets albumReview according to ID
        /// </summary>
        /// <param name="albumReviewId">albumReview ID</param>
        /// <returns>The albumReview</returns>
        AlbumReviewDTO GetAlbumReview(int albumReviewId);

        /// <summary>
        /// Gets all albumReviews
        /// </summary>
        /// <param name="filter" requiredPage="required page">album filter</param>
        /// <param name="required page">page to show</param>
        /// <returns>all available albumReviews</returns>
        AlbumReviewListQueryResultDTO ListAllAlbumReviews(AlbumReviewFilter filter, int requiredPage);

        /// <summary>
        /// Adds a review
        /// </summary>
        /// <param name="reviewDTO">Review details</param>
        void AddReview(AlbumReviewDTO reviewDTO);        
    }
}
