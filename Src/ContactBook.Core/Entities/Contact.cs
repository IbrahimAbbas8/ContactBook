using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Entities
{
    public class Contact : BaseEntity
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public bool IsFavorite {  get; set; } = false;
        public string ContactPicture { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public bool IsActive {  get; set; } = true;
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string AppUserId { get; set; }
        public virtual AppUser? AppUser { get; set; }
    }
}
