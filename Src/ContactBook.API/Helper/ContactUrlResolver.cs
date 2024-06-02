using AutoMapper;
using ContactBook.Core.Dtos;
using ContactBook.Core.Entities;

namespace ContactBook.API.Helper
{
    public class ContactUrlResolver : IValueResolver<Contact, ContactDto, string>
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// To return the URL correctly for operation
        /// </summary>
        /// <param name="configuration"></param>
        public ContactUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(Contact source, ContactDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ContactPicture))
            {
                return configuration["ApiURL"] + source.ContactPicture;
            }
            return null;
        }
    }
}
