﻿
using System;
using System.ComponentModel.DataAnnotations;

namespace BL.DTOs.Clients
{
    public class ClientDTO
    {
        public int ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public Guid UserAccountID { get; set; }

        public int[] SonglistIDs { get; set; }

        public override string ToString()
        {
            return this.FirstName +" " +this.LastName;
        }

    }
}
