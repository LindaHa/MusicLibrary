using BL.DTOs.Common;
using BL.DTOs.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Genres
{
    /// <summary>
    /// Wrapper for genre list query result with paging related data
    /// </summary>
    public class GenreListQueryResultDTO : PagedListQueryResultDTO<GenreDTO>
    {
        /// <summary>
        /// Filter used in this query
        /// </summary>
        public GenreFilter Filter { get; set; }
    }
}
