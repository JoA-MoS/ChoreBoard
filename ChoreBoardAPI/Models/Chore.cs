using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoreBoardAPI.Models
{
    public class Chore : AuditEntity, IHaveAnOwner
    {
        public int ChoreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BoardId { get; set; }
        public Board Board { get; set; }
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
    }
}
