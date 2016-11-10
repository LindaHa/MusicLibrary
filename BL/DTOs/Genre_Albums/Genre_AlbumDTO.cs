using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Genre_Albums
{
    public class Genre_AlbumDTO
    {
        public int ID { get; set; }

        [Required]
        public bool IsOfficial { get; set; }

        [Required]
        public int GenreID { get; set; }

        [Required]
        public int AlbumID { get; set; }

        public int CreatorID { get; set; }

        public override string ToString()
        {
            return this.GenreID + " : " + this.AlbumID;
        }
    }
}
