using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Filters
{
    public class SonglistFilter
    {
        public string Name { get; set; }
        public int SongID { get; set; }
        public int[] OwnerIDs { get; set; }
        public int OwnerID { get; set; }
        public bool SortAscending { get; set; }
    }
}
