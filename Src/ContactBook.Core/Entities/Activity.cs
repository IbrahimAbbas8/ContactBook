using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Entities
{
    public class Activity : BaseEntity
    {

        public string Contact { get; set; }

        public DateTime Date { get; set; }

        public string Action { get; set; }

        public string User { get; set; }
        public Activity()
        {
            Date = DateTime.UtcNow;
        }
    }
}
