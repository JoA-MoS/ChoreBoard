using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoreBoardAPI.Models
{
    public interface IHaveAnOwner
    {
        string OwnerId { get; set; }
        ApplicationUser Owner { get; set; }
    }
}
