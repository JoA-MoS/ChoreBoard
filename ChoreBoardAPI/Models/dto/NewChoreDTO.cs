using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoreBoardAPI.Models.dto
{
    public class NewChoreDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int BoardId { get; set; }
    }
}
