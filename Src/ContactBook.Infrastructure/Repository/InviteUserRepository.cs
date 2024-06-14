using AutoMapper;
using ContactBook.Core.Dtos;
using ContactBook.Core.Entities;
using ContactBook.Core.Helper;
using ContactBook.Core.Interfaces;
using ContactBook.Core.Sharing;
using ContactBook.Infrastructure.Data;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Infrastructure.Repository
{
    public class InviteUserRepository : GenericRepository<InviteUser>, IInviteUserRepository
    {
        private readonly ContactBookDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly EmailSettings emailSettings;

        public InviteUserRepository(ContactBookDbContext context, IMapper mapper, UserManager<AppUser> userManager, IOptions<EmailSettings> options) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.emailSettings = options.Value;
        }


        public async Task<IList<InviteUserDto>> GetAllAsync(Params Params, int ProfileId)
        {
            var query = await context.InviteUsers
                .AsNoTracking()
                .Where(c => c.ProfileId == ProfileId)
                .ToListAsync();

            // Search
            if (!string.IsNullOrEmpty(Params.Search))
            {
                query = query.Where(p => p.FirstName.ToLower().StartsWith(Params.Search)).ToList();
            }



            // paging
            query = query.Skip((Params.PageSize) * (Params.PageNumber - 1)).Take(Params.PageSize).ToList();

            var result = mapper.Map<List<InviteUserDto>>(query);
            return result;
        }




        public async Task<bool> AddAsyncInvite(InviteUser invite)
        {

            // Create User
            var user = new AppUser { Email = invite.Email, UserName = invite.Email, FirstName = invite.FirstName, LastName = invite.LastName, IsInvite = true, ProfileId = invite.ProfileId };
            var res = await userManager.CreateAsync(user, "P@$$w0rd");
            if (res.Succeeded == false)
            {
                return false;
            }
            var rol = await userManager.AddToRoleAsync(user, invite.UserType.ToString());
            if (rol.Succeeded == false)
            {
                return false;
            }


            // Add inviteUser
            await context.InviteUsers.AddAsync(invite);
            context.SaveChanges();

            


            // Send Email
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(invite.Email));

            email.Subject = "Your Account For Login";
            var builder = new BodyBuilder();
            builder.HtmlBody = $"Your Email: {invite.Email} and password: P@$$w0rd";
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);


            return true;
        }
    }
}
