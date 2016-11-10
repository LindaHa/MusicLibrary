using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Songs
{
    public class SongDTO
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        public double Duration { get; set; }

        [Required]
        public bool IsOfficial { get; set; }

        public int[] ReviewIDs { get; set; }

        //If the Song doen't belong to an Album from a particular Artist in real life, 
        //it will belong in the Unknown Album from the Artist in this App.
        [Required]
        public int AlbumID { get; set; }

        [Range(0.0, 10.0)]
        public double? AverageRating { get; set; }

        public int CreatorID { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
