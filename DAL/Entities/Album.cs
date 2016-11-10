using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Album : IEntity<int>
    {        
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        
        public virtual List<AlbumReview> Reviews { get; set; }

        [Required]
        public bool IsOfficial { get; set; }

        public virtual List<Song> Songs { get; set; }
       
        [Required]
        public virtual Artist Artist { get; set; }

        public virtual Client Creator { get; set; }
    }
}
