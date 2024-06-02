using System.ComponentModel.DataAnnotations;

namespace ContactBook.Core.Sharing
{
    public class EmailRequest : EmailRequestPDF
    {
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
    }
}
