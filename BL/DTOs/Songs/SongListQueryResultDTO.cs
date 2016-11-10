using BL.DTOs.Common;
using BL.DTOs.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Songs
{
    /// <summary>
    /// Wrapper for aong list query result with paging related data
    /// </summary>
    public class SongListQueryResultDTO : PagedListQueryResultDTO<SongDTO>
    {
        /// <summary>
        /// Filter used in this query
        /// </summary>
        public SongFilter Filter { get; set; }
    }
}
