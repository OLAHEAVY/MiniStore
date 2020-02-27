using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Data.Entities
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string City { get; set; }

        public string State { get; set; }

        public string Address { get; set; }

        public DateTime DateOfBirth {get;set;}

        public string Gender { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
