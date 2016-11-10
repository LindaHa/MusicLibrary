using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    //The relationship between an Album and it's Genres and wise versa, 
    //since an Album might have more than one Genre
    public class Genre_Album : IEntity<int>
    {
        public int ID { get; set; }

        [Required]
        public bool IsOfficial { get; set; }

        [Required]
        public virtual Genre Genre { get; set; }
        
        [Required]
        public virtual Album Album { get; set; }
        
        public virtual Client Creator { get; set; }
    }
}
