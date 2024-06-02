using AutoMapper;
using ContactBook.API.Extensions;
using ContactBook.API.Helper;
using ContactBook.Core.Dtos;
using ContactBook.Core.Entities;
using ContactBook.Core.Interfaces;
using ContactBook.Core.Sharing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InviteUserController : ControllerBase
    {
        private readonly IInviteUserRepository _repository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper mapper;

        public InviteUserController(IInviteUserRepository repository, UserManager<AppUser> userManager, IMapper mapper)
        {
            _repository = repository;
            _userManager = userManager;
            this.mapper = mapper;
        }


        /// <summary>
        /// Get a list of available InviteUsers
        /// </summary>
        /// <param name="Params">An object containing data for searching and pagination</param>
        /// <returns>List of InviteUsers</returns>
        [Authorize]
        [HttpGet("get-all-inviteUser")]
        public async Task<ActionResult> Get([FromQuery] Params Params)
        {
            var AccountId = _userManager.FindUserId(HttpContext.User);
            var inviteUser = await _repository.GetAllAsync(Params, AccountId.Id);
            if (inviteUser is not null)
            {
                return Ok(new Pagination<InviteUserDto>(Params.PageNumber, Params.PageSize, inviteUser));
            }
            return BadRequest();
        }


        /// <summary>
        /// Get a specific inviteUser by id
        /// </summary>
        /// <param name="id">ID of the requested inviteUser</param>
        /// <returns>inviteUser</returns>
        [HttpGet("get-inviteUser-by-id/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var inviteUser = await _repository.GetByIdAsync(id, p => p.AppUser);
            if (inviteUser is not null)
            {
                var res = mapper.Map<InviteUserDto>(inviteUser);
                return Ok(res);
            }
            return BadRequest($"Not Found This Id [{id}]");
        }


        /// <summary>
        /// Add a inviteUser
        /// </summary>
        /// <param name="inviteUserDto">The inviteUser data transfer object containing the details of the contact to be added</param>
        /// <returns>True Or False</returns>
        [Authorize]
        [HttpPost("add-new-inviteUser")]
        public async Task<ActionResult> Post(CreateInviteUserDto inviteUserDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var AccountId = _userManager.FindUserId(HttpContext.User);
                    var invit = mapper.Map<InviteUser>(inviteUserDto);
                    invit.AppUserId = AccountId.Id;
                    await _repository.AddAsync(invit);
                    return Ok(inviteUserDto);
                }
                return BadRequest(inviteUserDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        /// <summary>
        /// Update a inviteUser
        /// </summary>
        /// <param name="id">The key of the inviteUser to be modified</param>
        /// <param name="inviteUserDto">An object containing data for searching and pagination</param>
        /// <returns>True Or False</returns>
        [Authorize]
        [HttpPut("update-exiting-inviteUser-by-id/{id}")]
        public async Task<ActionResult> Put(int id, UpdateInviteUserDto inviteUserDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id != inviteUserDto.Id) return BadRequest();
                    var AccountId = _userManager.FindUserId(HttpContext.User);
                    var invit = mapper.Map<InviteUser>(inviteUserDto);
                    invit.AppUserId = AccountId.Id;
                    await _repository.UpdateAsync(id, invit);
                    return Ok(inviteUserDto);
                }
                return BadRequest(inviteUserDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Delete a inviteUser by id
        /// </summary>
        /// <param name="id">The key of the inviteUser to be Deleted</param>
        /// <returns>True Or False</returns>
        [Authorize]
        [HttpDelete("delete-inviteUser-by-id/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.DeleteAsync(id);
                    return Ok("The deletion was completed successfully");
                }
                return NotFound($"Not Found This Id [{id}]");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
