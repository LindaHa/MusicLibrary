using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Artists
{
    public class ArtistDTO
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Info { get; set; }

        [Required]
        public bool IsOfficial { get; set; }

        public List<int> AlbumIDs { get; set; }

        public int CreatorID { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
