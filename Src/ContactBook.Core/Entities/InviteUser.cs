using ContactBook.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Entities
{
    public class InviteUser : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public StatusUser StatusUser { get; set; }
        public UserType UserType { get; set; }
        public string AppUserId { get; set; }
        public virtual AppUser? AppUser { get; set; }
    }
}
