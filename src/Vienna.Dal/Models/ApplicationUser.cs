using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Vienna.Dal.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FullName => $@"{FirstName} {LastName}";
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        
    }
}
