using ContactBook.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Entities
{
    public class Profile : BaseEntity
    {
        public string CompanyName { get; set; }
        public int VatNumber { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Country Country { get; set; }
        public string AppUserId { get; set; }
        public virtual List<Contact> Contacts { get; set; } = new List<Contact>();
        public virtual List<InviteUser> inviteUsers { get; set; } = new List<InviteUser>();
        public virtual List<AppUser> AppUsers { get; set; }
    }
}
