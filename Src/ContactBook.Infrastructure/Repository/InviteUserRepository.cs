using AutoMapper;
using ContactBook.Core.Dtos;
using ContactBook.Core.Entities;
using ContactBook.Core.Interfaces;
using ContactBook.Core.Sharing;
using ContactBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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

        public InviteUserRepository(ContactBookDbContext context, IMapper mapper) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IList<InviteUserDto>> GetAllAsync(Params Params, string AccountId)
        {
            var query = await context.InviteUsers
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

            var result = mapper.Map<List<InviteUserDto>>(query);
            return result;
        }
    }
}
