using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChoreBoardAPI.Models
{

    public abstract class AuditEntity : BaseEntity
    {
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string CreatedById { get; set; }
        // // [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { get; set; }


        public DateTime Modified { get; set; } = DateTime.UtcNow;
        public string ModifiedById { get; set; }
        // // [ForeignKey("ModifiedById")]
        public ApplicationUser ModifiedBy { get; set; }
    }
    
}
