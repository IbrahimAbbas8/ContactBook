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
        public StatusUser StatusUser { get; set; } = StatusUser.Pending;
        public UserType UserType { get; set; }
        public int ProfileId { get; set; }
        public virtual Profile? Profile { get; set; }
    }
}
