using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Songlist : IEntity<int>
    {
        public int ID { get; set; }

        [Required][MaxLength(256)]
        public string Name { get; set; }

        public virtual List<Song> Songs { get; set; }
        
        [Required]
        public Client Owner { get; set; }
    }
}
