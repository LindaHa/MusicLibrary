using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.AlbumReviews
{
    public class AlbumReviewDTO
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Text { get; set; }

        public int CreatorID { get; set; }

        [Required]
        public int AlbumID { get; set; }

        public override string ToString()
        {
            return this.Text;
        }
    }
}
