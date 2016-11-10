using BL.DTOs.Common;
using BL.DTOs.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.AlbumReviews
{
    /// <summary>
    /// Wrapper for album review list query result with paging related data
    /// </summary>
    public class AlbumReviewListQueryResultDTO : PagedListQueryResultDTO<AlbumReviewDTO>
    {
        /// <summary>
        /// Filter used in this query
        /// </summary>
        public AlbumReviewFilter Filter { get; set; }
    }
}
