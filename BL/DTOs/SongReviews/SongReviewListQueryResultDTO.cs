using BL.DTOs.Common;
using BL.DTOs.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.SongReviews
{
    /// <summary>
    /// Wrapper for song list query result with paging related data
    /// </summary>
    public class SongReviewListQueryResultDTO : PagedListQueryResultDTO<SongReviewDTO>
    {
        /// <summary>
        /// Filter used in this query
        /// </summary>
        public SongReviewFilter Filter { get; set; }
    }
}
