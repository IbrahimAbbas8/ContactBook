using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ContactBook.API.Extensions;
using ContactBook.API.Helper;
using ContactBook.Core.Dtos;
using ContactBook.Core.Entities;
using ContactBook.Core.Interfaces;
using ContactBook.Core.Sharing;
using ContactBook.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository _repository;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPdfService pdfService;
        private readonly IActivityRepository activityRepository;

        public ContactController(IContactRepository repository, IMapper mapper, UserManager<AppUser> userManager, IPdfService pdfService, IActivityRepository activityRepository)
        {
            _repository = repository;
            this.mapper = mapper;
            this._userManager = userManager;
            this.pdfService = pdfService;
            this.activityRepository = activityRepository;
        }


        /// <summary>
        /// Get a list of available Contacts
        /// </summary>
        /// <param name="Params">An object containing data for searching and pagination</param>
        /// <returns>List of Contacts</returns>
        [Authorize]
        [HttpGet("get-all-contact")]
        public async Task<ActionResult> Get([FromQuery] Params Params)
        {
            var AccountId = _userManager.FindUserByClaimPrincipalWithProfile(HttpContext.User).Result;
            var contact = await _repository.GetAllAsync(Params, AccountId.Profile.Id);
            if (contact is not null)
            {
                return Ok(new Pagination<ContactDto>(Params.PageNumber, Params.PageSize, contact));
            }
            return BadRequest();
        }

        /// <summary>
        /// Get a specific contact by id
        /// </summary>
        /// <param name="id">ID of the requested contact</param>
        /// <returns>contact</returns>
        [HttpGet("get-contact-by-id/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var AccountId = _userManager.FindUserId(HttpContext.User);
            var contact = await _repository.GetByIdAsync(id, p => p.Profile);
            if (contact is not null)
            {
                var res = mapper.Map<ContactDto>(contact);
                await activityRepository.AddAsync(new Activity 
                { 
                    Contact = $"{contact.FirstName} {contact.LastName}", 
                    Action = "Access",
                    User = $"{AccountId.FirstName} {AccountId.LastName}" 
                });
                return Ok(res);
            }
            return BadRequest($"Not Found This Id [{id}]");
        }



        /// <summary>
        /// Add a contact
        /// </summary>
        /// <param name="contactDto">The contact data transfer object containing the details of the contact to be added</param>
        /// <returns>True Or False</returns>
        [Authorize]
        [HttpPost("add-new-contact")]
        public async Task<ActionResult> Post(CreateContactDto contactDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var AccountId = _userManager.FindUserByClaimPrincipalWithProfile(HttpContext.User).Result;
                    var res = await _repository.AddAsync(contactDto, AccountId.Profile.Id);
                    await activityRepository.AddAsync(new Activity
                    {
                        Contact = $"{contactDto.FirstName} {contactDto.LastName}",
                        Action = "Add",
                        User = $"{AccountId.FirstName} {AccountId.LastName}"
                    });
                    return res ? Ok(contactDto) : BadRequest();
                }
                return BadRequest(contactDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        /// <summary>
        /// Update a contact
        /// </summary>
        /// <param name="id">The key of the contact to be modified</param>
        /// <param name="contactDto">An object containing data for searching and pagination</param>
        /// <returns>True Or False</returns>
        [Authorize]
        [HttpPut("update-exiting-contact-by-id/{id}")]
        public async Task<ActionResult> Put(int id, UpdateContactDto contactDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var AccountId = _userManager.FindUserId(HttpContext.User);
                    await activityRepository.AddAsync(new Activity
                    {
                        Contact = $"{contactDto.FirstName} {contactDto.LastName}",
                        Action = "Update",
                        User = $"{AccountId.FirstName} {AccountId.LastName}"
                    });
                    var res = await _repository.UpdateAsync(id, contactDto);
                    return res ? Ok(contactDto) : BadRequest(res);
                }
                return BadRequest(contactDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Delete a conteact by id
        /// </summary>
        /// <param name="id">The key of the contact to be Deleted</param>
        /// <returns>True Or False</returns>
        [Authorize]
        [HttpDelete("delete-contact-by-id/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _repository.DeleteAsyncWithImage(id);
                    if (res.Item1)
                    {
                        var AccountId = _userManager.FindUserId(HttpContext.User);
                        await activityRepository.AddAsync(new Activity
                        {
                            Contact = $"{res.Item2.FirstName} {res.Item2.LastName}",
                            Action = "Update",
                            User = $"{AccountId.FirstName} {AccountId.LastName}"
                        });
                        return Ok(res.Item1);
                    }
                    return BadRequest(res.Item1);
                }
                return NotFound($"Not Found This Id [{id}]");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Export data to pdf
        /// </summary>
        /// <param name="Params">An object containing data for searching and pagination</param>
        /// <returns>File</returns>
        [Authorize]
        [HttpGet("export-to-pdf")]
        public async Task<IActionResult> ExportToPDF([FromQuery] Params Params)
        {
            var AccountId = _userManager.FindUserByClaimPrincipalWithProfile(HttpContext.User).Result;
            var contacts = await _repository.GetAllAsync(Params, AccountId.Profile.Id);
            var pdfBytes = pdfService.CreatePdf(contacts);
            return File(pdfBytes, "application/pdf", "contacts.pdf");
        }
    }
}
