using BL.DTOs.Filters;
using BL.DTOs.SongReviews;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.SongReviews
{
    /// <summary>
    /// Handles SongReview CRUD
    /// </summary>  
    public interface ISongReviewService : ICreator
    {
        /// <summary>
        /// Creates new songReview
        /// </summary>
        /// <param name="songReviewDTO">songReview details</param>
        void CreateSongReview(SongReviewDTO songReviewDTO);

        /// <summary>
        /// Adds a review
        /// </summary>
        /// <param name="reviewDTO">Review details</param>
        void AddReview(SongReviewDTO reviewDTO);

        /// <summary>
        /// Updates SongReview according to ID
        /// </summary>
        /// <param name="SongReviewDTO">songReview details</param>
        void EditSongReview(SongReviewDTO songReviewDTO);

        /// <summary>
        /// Removes songReview according to ID
        /// </summary>
        /// <param name="songReviewID">songReview ID</param>
        void DeleteSongReview(int songReviewID);

        /// <summary>
        /// Gets songReview according to ID
        /// </summary>
        /// <param name="songReviewID">songReview ID</param>
        /// <returns>The songReview</returns>
        SongReviewDTO GetSongReview(int songReviewID);


        /// <summary>
        /// Gets all songReviews
        /// </summary>
        /// <param name="filter">album filter</param>
        /// <param name="required page">page to show</param>
        /// <returns>all available songReviews</returns>
        SongReviewListQueryResultDTO ListAllSongReviews(SongReviewFilter filter, int requiredPage);


    }
}
