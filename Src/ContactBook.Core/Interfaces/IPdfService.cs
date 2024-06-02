using ContactBook.Core.Dtos;
using ContactBook.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Interfaces
{
    public interface IPdfService
    {
        byte[] CreatePdf(IEnumerable<ContactDto> contacts);
    }
}
