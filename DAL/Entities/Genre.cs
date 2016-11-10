using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Genre : IEntity<int>
    {
        public int ID { get; set; }

        [Required][MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Info { get; set; }

        [Required]
        public bool IsOfficial { get; set; }

        public virtual Client Creator { get; set; }

    }
}
