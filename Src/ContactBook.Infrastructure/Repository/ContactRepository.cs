using AutoMapper;
using ContactBook.Core.Dtos;
using ContactBook.Core.Entities;
using ContactBook.Core.Interfaces;
using ContactBook.Core.Sharing;
using ContactBook.Infrastructure.Data;  
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Infrastructure.Repository
{
    public class ContactRepository : GenericRepository<Contact> , IContactRepository
    {
        private readonly ContactBookDbContext context;
        private readonly IFileProvider fileProvider;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public ContactRepository(ContactBookDbContext context, IFileProvider fileProvider, IMapper mapper) : base(context)
        {
            this.context = context;
            this.fileProvider = fileProvider;
            this.mapper = mapper;
        }

        public async Task<IList<ContactDto>> GetAllAsync(Params Params, string AccountId)
        {
            var query = await context.Contacts
                .AsNoTracking()
                .Where(c => c.AppUserId == AccountId)
                .ToListAsync();

            // Search
            if (!string.IsNullOrEmpty(Params.Search))
            {
                query = query.Where(p => p.FirstName.ToLower().StartsWith(Params.Search)).ToList();
            }

                

            // paging
            query = query.Skip((Params.PageSize) * (Params.PageNumber - 1)).Take(Params.PageSize).ToList();

            var result = mapper.Map<List<ContactDto>>(query);
            return result;
        }

        public async Task<bool> AddAsync(CreateContactDto dto, string AccountId)
        {
            if (dto.Image is not null)
            {
                var root = "/images/Contacts/";
                var contactName = $"{Guid.NewGuid()}" + dto.Image.FileName;
                if (!Directory.Exists("wwwroot" + root))
                {
                    Directory.CreateDirectory("wwwroot" + root);
                }
                var src = root + contactName;
                var picInfo = fileProvider.GetFileInfo(src);
                var rootPath = picInfo.PhysicalPath;
                await Console.Out.WriteLineAsync(rootPath);
                using (var fileStream = new FileStream(rootPath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(fileStream);
                }

                var res = mapper.Map<Contact>(dto);
                res.ContactPicture = src;
                res.AppUserId = AccountId;
                await context.Contacts.AddAsync(res);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> UpdateAsync(int id, UpdateContactDto dto)
        {
            var currentContact = await context.Contacts.FindAsync(id);
            if (currentContact is not null)
            {
                var src = "";
                if (dto.Image is not null)
                {
                    var root = "/images/Contacts/";
                    var contactName = $"{Guid.NewGuid()}" + dto.Image.FileName;
                    if (!Directory.Exists("wwwroot" + root))
                    {
                        Directory.CreateDirectory("wwwroot" + root);
                    }
                    src = root + contactName;
                    var picInfo = fileProvider.GetFileInfo(src);
                    var rootPath = picInfo.PhysicalPath;
                    await Console.Out.WriteLineAsync(rootPath);
                    using (var fileStream = new FileStream(rootPath, FileMode.Create))
                    {
                        await dto.Image.CopyToAsync(fileStream);
                    }
                }

                // Remove Old ContactPicture
                if (!string.IsNullOrEmpty(currentContact.ContactPicture))
                {
                    var picInfo = fileProvider.GetFileInfo(currentContact.ContactPicture);
                    var rootPath = picInfo.PhysicalPath;
                    System.IO.File.Delete(rootPath);
                }

                // Update Contact
                mapper.Map(dto, currentContact);
                currentContact.ContactPicture = src;
                context.Update(currentContact);
                await context.SaveChangesAsync();

                return true;
            }
            return false;
        }


        public async Task<(bool,Contact)> DeleteAsyncWithImage(int id)
        {
            var res = await context.Contacts.FindAsync(id);

            if (res is not null)
            {
                if (!string.IsNullOrEmpty(res.ContactPicture))
                {
                    var picInfo = fileProvider.GetFileInfo(res.ContactPicture);
                    var rootPath = picInfo.PhysicalPath;
                    System.IO.File.Delete(rootPath);
                }

                context.Contacts.Remove(res);
                context.SaveChanges();
                return (true,res);
            }
            return (false, res);
        }

    }
}
