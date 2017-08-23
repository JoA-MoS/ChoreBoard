using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChoreBoardAPI.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

    

        [NotMapped]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        [InverseProperty("Owner")]
        public virtual List<Board> Boards { get; set; } = new List<Board>();

        [InverseProperty("Owner")]
        public virtual List<Chore> Chores { get; set; } = new List<Chore>();
    }
}
