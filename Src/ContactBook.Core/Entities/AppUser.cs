using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<Contact> Contacts { get; set; } = new List<Contact>();
        public virtual List<InviteUser> inviteUsers { get; set; } = new List<InviteUser>();
        public virtual Profile Profile { get; set; }
    }
}
