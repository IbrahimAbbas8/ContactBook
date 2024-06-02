using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Dtos
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsFavorite { get; set; }
        public string ContactPicture { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string AccountName { get; set;}
    }
}
