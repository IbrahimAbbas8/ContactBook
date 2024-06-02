using AutoMapper;
using ContactBook.API.Extensions;
using ContactBook.Core.Dtos;
using ContactBook.Core.Entities;
using ContactBook.Core.Helper;
using ContactBook.Core.Interfaces;
using ContactBook.Core.Sharing;
using ContactBook.Infrastructure.Repository;
using ContactBook.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ContactBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;
        private readonly IContactRepository repository;
        private readonly UserManager<AppUser> userManager;
        private readonly IPdfService pdfServic;
        private readonly IActivityRepository activity;
        private readonly EmailSettings emailSettings;

        public EmailController(IEmailService emailService, 
            IContactRepository repository, 
            UserManager<AppUser> userManager, 
            IPdfService pdfServic,
            IActivityRepository activity,
            IOptions<EmailSettings> options)
        {
            this.emailService = emailService;
            this.repository = repository;
            this.userManager = userManager;
            this.pdfServic = pdfServic;
            this.activity = activity;
            this.emailSettings = options.Value;
        }

        /// <summary>
        /// Send a text message to an email
        /// </summary>
        /// <param name="emailRequest">Data necessary for the transmission process</param>
        /// <returns></returns>
        [HttpPost("send-text")]
        public async Task<IActionResult> SendEmail(EmailRequest emailRequest)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    await emailService.SendEmailAsync(emailRequest);
                    var AccountId = userManager.FindUserId(HttpContext.User);
                    await activity.AddAsync(new Activity
                    {
                        Contact = emailSettings.Displayname,
                        Action = "Update",
                        User = $"{AccountId.FirstName} {AccountId.LastName}"
                    });
                    return Ok();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return BadRequest();
        }


        /// <summary>
        /// Send a PDF file to an email
        /// </summary>
        /// <param name="emailRequest">The email sent to him</param>
        /// <param name="Params">An object containing data for searching and pagination</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("send-pdf")]
        public async Task<IActionResult> SendEmailPDF(EmailRequestPDF emailRequest, [FromQuery] Params Params)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var arrayByte = ExportToPDF(Params).Result;
                    await emailService.SendEmailPDFAsync(emailRequest, arrayByte);
                    return Ok();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return BadRequest();
        }


        /// <summary>
        /// Generate the file to send by email
        /// </summary>
        /// <param name="Params">An object containing data for searching and pagination</param>
        /// <returns>Array byte</returns>
        private async Task<byte[]> ExportToPDF([FromQuery] Params Params)
        {
            var AccountId = userManager.FindUserId(HttpContext.User);
            var contacts = await repository.GetAllAsync(Params, AccountId.Id);
            var pdfBytes = pdfServic.CreatePdf(contacts);
            return pdfBytes;
        }
    }
}
