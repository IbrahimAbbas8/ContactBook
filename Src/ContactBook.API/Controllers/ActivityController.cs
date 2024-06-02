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
    public class ActivityController : ControllerBase
    {
        private readonly IActivityRepository repository;

        public ActivityController(IActivityRepository repository)
        {
            this.repository = repository;
        }


        /// <summary>
        /// Get a list of available activities
        /// </summary>
        /// <param name="Params">An object containing data for pagination</param>
        /// <returns>List of activities</returns>
        [Authorize]
        [HttpGet("get-all-activity")]
        public async Task<ActionResult> Get([FromQuery] ActivityParams Params)
        {
            var activity = await repository.GetAllAsync(Params);
            if (activity is not null)
            {
                return Ok(new Pagination<Activity>(Params.PageNumber, Params.PageSize, activity));
            }
            return BadRequest();
        }
    }
}
