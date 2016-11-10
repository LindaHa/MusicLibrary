using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Songlists
{
    public class SonglistDTO
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public List<int> SongIDs { get; set; }

        [Required]
        public int OwnerID { get; set; }

        public override string ToString()
        {
            return this.Name + ": " + this.SongIDs.Count() + " songs";
        }
    }
}
