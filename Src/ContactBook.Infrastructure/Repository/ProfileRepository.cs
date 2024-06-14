using ContactBook.Core.Entities;
using ContactBook.Core.Interfaces;
using ContactBook.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Infrastructure.Repository
{
    internal class ProfileRepository : GenericRepository<Profile> , IProfileRepository
    {
        public ProfileRepository(ContactBookDbContext context) : base(context)
        {
        }
    }
}
