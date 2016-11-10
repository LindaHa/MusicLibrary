using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Common
{
    /// <summary>
    /// Base class for DTO paged query results
    /// </summary>
    public abstract class PagedListQueryResultDTO<T>
    {
        /// <summary>
        /// Total number of products for the query
        /// </summary>
        public int TotalResultCount { get; set; }

        /// <summary>
        /// Number of page (indexed from 1) which was requested
        /// </summary>
        public int RequestedPage { get; set; }

        /// <summary>
        /// The query results page
        /// </summary>
        public IEnumerable<T> ResultsPage { get; set; }
    }
}
