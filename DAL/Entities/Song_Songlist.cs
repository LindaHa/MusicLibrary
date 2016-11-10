using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Song_Songlist : IEntity<int>
    {
        public int ID { get; set; }


        [Required]
        public virtual Song Song { get; set; }


        [Required]
        public virtual Songlist Songlist { get; set; }

        
        public virtual Client Creator { get; set; }
    }
}
