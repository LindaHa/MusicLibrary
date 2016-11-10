using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Filters
{
    public class Genre_AlbumFilter
    {
        public int AlbumID { get; set; }
        public int GenreID { get; set; }
        public bool IsOfficial { get; set; }
        public List<int> CreatorIDs { get; set; }
        public int ArtistID { get; set; }
    }
}
