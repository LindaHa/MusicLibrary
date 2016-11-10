using Riganti.Utils.Infrastructure.Core;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class AlbumReview : IEntity<int>
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Text { get; set; }
        
        public virtual Client Creator { get; set; }

        [Required]
        public virtual Album Album { get; set; }

        [Required]
        [Range(0.0, 10.0)]
        public double UserRating { get; set; }

    }
}
