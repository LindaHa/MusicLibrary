using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Filters
{
    public class SongFilter
    {
        public string Name { get; set; }
        public int[] ArtistIDs { get; set; }
        public int AlbumID { get; set; }
        public TimeSpan MinimalDuration { get; set; }
        public TimeSpan MaximalDuration { get; set; }
        public int[] GenreIDs { get; set; }
        public bool SortAscending { get; set; }

        [Range(0.0, 10.0)]
        public int[] CreatorIDs { get; set; }
        public double MinimalRating { get; set; }
    }
}
