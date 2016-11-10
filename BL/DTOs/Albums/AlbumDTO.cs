using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Albums
{
    public class AlbumDTO
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public int[] ReviewIDs { get; set; }

        [Required]
        public bool IsOfficial { get; set; }

        public int[] SongIDs { get; set; }

        [Required]
        public int ArtistID { get; set; }

        public int CreatorID { get; set; }

        [Range(0.0, 10.0)]
        public double? AverageRating { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
