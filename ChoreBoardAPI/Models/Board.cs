using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoreBoardAPI.Models
{
    public class Board : AuditEntity, IHaveAnOwner
    {
        public int BoardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public virtual List<Chore> Chores { get; set; } = new List<Chore>();
    }
}
