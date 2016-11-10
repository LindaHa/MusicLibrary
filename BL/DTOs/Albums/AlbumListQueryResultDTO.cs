using BL.DTOs.Common;
using BL.DTOs.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Albums
{
    /// <summary>
    /// Wrapper for album list query result with paging related data
    /// </summary>
    public class AlbumListQueryResultDTO : PagedListQueryResultDTO<AlbumDTO>
    {
        /// <summary>
        /// Filter used in this query
        /// </summary>
        public AlbumFilter Filter { get; set; }
    }
}
