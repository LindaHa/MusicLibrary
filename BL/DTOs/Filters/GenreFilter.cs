using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Filters
{
    public class GenreFilter
    {
        public string Name { get; set; }
        public int ArtistID { get; set; }
        public List<int> CreatorIDs { get; set; }
        public bool SortAscending { get; set; }
    }
}
