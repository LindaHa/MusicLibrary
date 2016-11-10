using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Song : IEntity<int>
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }
        [Required]
        public bool IsOfficial { get; set; }

        public virtual List<SongReview> Reviews { get; set; }

        //If the Song doen't belong to an Album from a particular Artist in real life, 
        //it will belong in the Unknown Album from the Artist in this App.
        [Required] 
        public virtual Album Album { get; set; }

        public virtual Client Creator { get; set; }
    }
}
