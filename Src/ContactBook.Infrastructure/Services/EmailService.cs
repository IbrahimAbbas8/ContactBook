using ContactBook.Core.Helper;
using ContactBook.Core.Interfaces;
using ContactBook.Core.Sharing;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            this.emailSettings = options.Value;
        }
        /// <summary>
        /// Send a text mail
        /// </summary>
        /// <param name="emailRequest">Mail information</param>
        /// <returns></returns>
        public async Task SendEmailAsync(EmailRequest emailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(emailRequest.To));
            if (!string.IsNullOrEmpty(emailRequest.CC) && emailRequest.CC != "string")
            {
                email.Cc.Add(MailboxAddress.Parse(emailRequest.CC));
            }
            if (!string.IsNullOrEmpty(emailRequest.BCC) && emailRequest.CC != "string")
            {
                email.Bcc.Add(MailboxAddress.Parse(emailRequest.BCC));
            }
            email.Subject = emailRequest.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = emailRequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }


        /// <summary>
        /// Send the PDF file via email
        /// </summary>
        /// <param name="emailRequestPDF">The email sent to him</param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public async Task SendEmailPDFAsync(EmailRequestPDF emailRequestPDF, byte[] bytes)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(emailRequestPDF.To));
            email.Subject = "File PDF";
            var builder = new BodyBuilder();
            

            builder.Attachments.Add("Contects.pdf", bytes, ContentType.Parse("application/octet-stream"));

            builder.HtmlBody = "This id File pdf to all contacts";
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
