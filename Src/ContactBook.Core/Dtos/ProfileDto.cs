using ContactBook.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Dtos
{
    public class ProfileDto
    {
        public string CompanyName { get; set; }
        public int VatNumber { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Country Country { get; set; }
    }
}
