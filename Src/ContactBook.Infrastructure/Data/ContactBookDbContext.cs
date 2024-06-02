using ContactBook.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Infrastructure.Data
{
    public class ContactBookDbContext : IdentityDbContext<AppUser>
    {
        public ContactBookDbContext(DbContextOptions<ContactBookDbContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<InviteUser> InviteUsers { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}
