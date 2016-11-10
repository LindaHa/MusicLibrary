using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Filters
{
    public class AlbumFilter
    {
        public string Name { get; set; }
        public List<int> ArtistIDs { get; set; }
        public List<int> GenreIDs { get; set; }
        public bool SortAscending { get; set; }

        [Range(0.0, 10.0)]
        public double MinimalRating { get; set; }
        public List<int> CreatorIDs { get; set; }

    }
}
