using ContactBook.Core.Dtos;
using ContactBook.Core.Entities;
using ContactBook.Core.Sharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Interfaces
{
    public interface IActivityRepository : IGenericRepository<Activity>
    {
        Task<IList<Activity>> GetAllAsync(ActivityParams Params);
    }
}
