using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Song_Songlists
{
    public class Song_SonglistDTO
    {
        public int ID { get; set; }


        [Required]
        public int SongID { get; set; }


        [Required]
        public int SonglistID { get; set; }


        public int CreatorID { get; set; }

        public override string ToString()
        {
            return this.SongID.ToString() + " : " + this.SonglistID.ToString();
        }
    }
}
