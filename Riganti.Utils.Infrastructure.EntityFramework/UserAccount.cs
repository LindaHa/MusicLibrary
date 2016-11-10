using System;
using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot.Relational;
using Riganti.Utils.Infrastructure.Core;

namespace Riganti.Utils.Infrastructure.EntityFramework
{
    /// <summary>
    /// This is NOT official part of Riganti.Utils.Infrastructure
    /// 
    /// Base class for application user, 
    /// all declared properties MUST be virtual
    /// 
    /// Please note this class needs to be referenced even from PL,
    /// therefore its placed apart from other entities in DAL.
    /// </summary>
    public class UserAccount : RelationalUserAccount , IEntity<Guid>
    {
        //[Required]
        //[MaxLength(64)]
        //public virtual string FirstName { get; set; }

        //[Required]
        //[MaxLength(64)]
        //public virtual string LastName { get; set; }

        //[Required]
        //[MaxLength(64)]
        //public virtual string Nickname { get; set; }
    }
}
