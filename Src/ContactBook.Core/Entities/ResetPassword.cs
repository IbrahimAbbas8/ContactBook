using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Entities
{
    public class ResetPassword
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string Password2 { get; set; }
    }
}
