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
    public interface IContactRepository : IGenericRepository<Contact>
    {
        Task<IList<ContactDto>> GetAllAsync(Params Params, int ProfileId);
        Task<bool> AddAsync(CreateContactDto dto, int ProfileId);
        Task<bool> UpdateAsync(int id, UpdateContactDto dto);
        Task<(bool, Contact)> DeleteAsyncWithImage(int id);
    }
}
