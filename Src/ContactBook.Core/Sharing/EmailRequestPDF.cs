using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Sharing
{
    public class EmailRequestPDF
    {
        [Required]
        public string To { get; set; }
        
    }
}
