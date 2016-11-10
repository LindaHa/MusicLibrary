using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Filters
{
    public class Song_SonglistFilter
    {
        public int SonglistID { get; set; }
        public List<int> CreatorIDs { get; set; }
        public int SongID { get; set; }
    }
}
