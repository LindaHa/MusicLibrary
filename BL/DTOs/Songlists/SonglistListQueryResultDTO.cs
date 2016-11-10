using BL.DTOs.Common;
using BL.DTOs.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Songlists
{
    /// <summary>
    /// Wrapper for songlist list query result with paging related data
    /// </summary>
    public class SonglistListQueryResultDTO : PagedListQueryResultDTO<SonglistDTO>
    {
        /// <summary>
        /// Filter used in this query
        /// </summary>
        public SonglistFilter Filter { get; set; }
    }
}
