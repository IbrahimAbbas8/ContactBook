using AutoMapper;
using ContactBook.API.Extensions;
using ContactBook.Core.Dtos;
using ContactBook.Core.Entities;
using ContactBook.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Claims;
using Profile = ContactBook.Core.Entities.Profile;

namespace ContactBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenServices tokenServices;
        private readonly IMapper mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenServices tokenServices, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenServices = tokenServices;
            this.mapper = mapper;
        }


        /// <summary>
        /// Login using your email and password
        /// </summary>
        /// <param name="dto">Email and password</param>
        /// <returns>FullName and email and token</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return Unauthorized("not authorize");
            }

            var res = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (res is null || res.Succeeded == false)
            {
                return Unauthorized("not authorize");
            }

            return Ok(new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = tokenServices.CreateToken(user),
            });
        }


        /// <summary>
        /// Create an account and profile
        /// </summary>
        /// <param name="dto">Account and profile information</param>
        /// <returns>FullName and email and token</returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new AppUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                Profile = new Profile
                {
                    CompanyName = dto.CompanyName,
                    VatNumber = dto.VatNumber,
                    Street = dto.Street,
                    Street2 = dto.Street2,
                    City = dto.City,
                    State = dto.State,
                    ZipCode = dto.ZipCode,
                    Country = dto.Country,
                }
            };
            var res = await userManager.CreateAsync(user, dto.Password);
            if (res.Succeeded is false)
            {
                BadRequest();
            }
            return Ok(new UserDto
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Token = tokenServices.CreateToken(user),
            });
        }



        /// <summary>
        /// To return the current user
        /// </summary>
        /// <returns>FullName and email and token</returns>
        [Authorize]
        [HttpGet("get-current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await userManager.FindEmailByClaimPrincipal(HttpContext.User);
            return Ok(new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user?.Email,
                Token = tokenServices.CreateToken(user)
            });
        }


        /// <summary>
        /// To check if the user exists via email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True or false</returns>
        //[Authorize]
        [HttpGet("check-email-exist")]
        public async Task<IActionResult> CheckEmailExist([FromQuery] string email)
        {
            return Ok(await userManager.FindByEmailAsync(email) != null);
        }


        /// <summary>
        /// Returns the current user profile
        /// </summary>
        /// <returns>Profile</returns>
        [Authorize]
        [HttpGet("get-user-profile")]
        public async Task<IActionResult> GetUserAddress()
        {
            var user = await userManager.FindUserByClaimPrincipalWithProfile(HttpContext.User);
             var profile = mapper.Map<ProfileDto>(user?.Profile);
            return Ok(profile);
        }


        /// <summary>
        /// Update current profile information
        /// </summary>
        /// <param name="dto">Profile information</param>
        /// <returns>Profile</returns>
        [Authorize]
        [HttpPut("update-user-profile")]
        public async Task<IActionResult> UpdateUserProfile(ProfileDto dto)
        {
            var user = await userManager.FindUserByClaimPrincipalWithProfile(HttpContext.User);
            user.Profile = mapper.Map<Profile>(dto);
            var res = await userManager.UpdateAsync(user);
            if (res.Succeeded) return Ok(mapper.Map<ProfileDto>(user.Profile));
            return BadRequest();
        }


        /// <summary>
        /// Verify if the email exists
        /// </summary>
        /// <param name="forgot">email</param>
        /// <returns>Email and true or false</returns>
        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgot)
        {
            var user = await userManager.FindByEmailAsync(forgot.Email);
            if (user is null)
            {
                return BadRequest(new IsEmail
                {
                    Email = forgot.Email,
                    IsFind = false,
                });
            }

            return Ok(new IsEmail
            {
                Email = forgot.Email,
                IsFind = true,
            });
        }


        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="reset">Data needed to reset the password</param>
        /// <returns>Returns the result</returns>
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPassword reset)
        {
            if (ModelState.IsValid)
            {
                if(reset.Password != reset.Password2)
                {
                    return BadRequest("Password mismatch");
                }
                var user = await userManager.FindByEmailAsync(reset.Email);
                if (user is null)
                {
                    return BadRequest("Not Found");
                }
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                var resetPassResult = await userManager.ResetPasswordAsync(user, token, reset.Password);
                if (!resetPassResult.Succeeded)
                {
                    return BadRequest(resetPassResult.Errors);
                }
            }
            return Ok(new { message = "The password has been reset successfully." });
        }



    }
}
