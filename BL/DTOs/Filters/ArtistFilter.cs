using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Filters
{
    public class ArtistFilter
    {
        public string Name { get; set; }
        public bool SortAscending { get; set; }

        [Range(0.0, 10.0)]
        public int[] CreatorIDs { get; set; }
        public double MinimalRating { get; set; }
    }
}
