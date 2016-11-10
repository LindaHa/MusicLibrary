using BL.DTOs.Common;
using BL.DTOs.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Artists
{
    /// <summary>
    /// Wrapper for artist list query result with paging related data
    /// </summary>
    public class ArtistListQueryResultDTO : PagedListQueryResultDTO<ArtistDTO>
    {
        /// <summary>
        /// Filter used in this query
        /// </summary>
        public ArtistFilter Filter { get; set; }
    }
}
