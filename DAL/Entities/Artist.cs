using Riganti.Utils.Infrastructure.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Artist : IEntity<int>
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Info { get; set; }

        [Required]
        public bool IsOfficial { get; set; }
        
        public virtual List<Album> Albums { get; set; }        

        public virtual Client Creator { get; set; }
    }
}
