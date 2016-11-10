using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Filters
{
    public class AlbumReviewFilter
    {
        public int AlbumID { get; set; }
        public int UserID { get; set; }
        public List<int> CreatorIDs { get; set; }
        public bool SortAscending { get; set; }
    }
}
