using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Filters
{
    public class SongReviewFilter
    {
        public int SongID { get; set; }
        public int UserID { get; set; }
        public List<int> CreatorIDs { get; set; }
        public bool SortAscending { get; set; }
    }
}
