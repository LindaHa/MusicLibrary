using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{    
    public class SongReview : IEntity<int>
    {
        public int ID { get; set; }

        [Required][MaxLength(1024)]
        public string Text { get; set; }

        public virtual Client Creator { get; set; }

        [Required]
        public virtual Song Song { get; set; }

        [Required]
        [Range(0.0, 10.0)]
        public double UserRating { get; set; }
    }
}
