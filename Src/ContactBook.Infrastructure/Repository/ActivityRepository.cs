using AutoMapper;
using ContactBook.Core.Dtos;
using ContactBook.Core.Entities;
using ContactBook.Core.Interfaces;
using ContactBook.Core.Sharing;
using ContactBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Infrastructure.Repository
{
    public class ActivityRepository : GenericRepository<Activity> , IActivityRepository
    {
        private readonly ContactBookDbContext context;

        public ActivityRepository(ContactBookDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IList<Activity>> GetAllAsync(ActivityParams Params)
        {
            var query = await context.Activities
                .AsNoTracking()
                .ToListAsync();



            // paging
            query = query.Skip((Params.PageSize) * (Params.PageNumber - 1)).Take(Params.PageSize).ToList();

            return query;
        }
    }
}
