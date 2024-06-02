using ContactBook.Core.Sharing;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest emailRequest);
        Task SendEmailPDFAsync(EmailRequestPDF emailRequestPDF, byte[] bytes);
    }
}
