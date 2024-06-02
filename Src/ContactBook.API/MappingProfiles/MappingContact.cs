using ContactBook.Core.Entities;
using AutoMapper;
using ContactBook.Core.Dtos;
using Profilee = AutoMapper.Profile;
using ContactBook.API.Helper;


namespace ContactBook.API.MappingProfiles
{
    public class MappingContact : Profilee
    {
        public MappingContact()
        {
            CreateMap<Contact, ContactDto>()
                .ForMember(x => x.AccountName, c => c.MapFrom(cd => $"{cd.AppUser.FirstName} {cd.AppUser.LastName}"))
                .ForMember(pd => pd.ContactPicture, p => p.MapFrom<ContactUrlResolver>())
                .ReverseMap();
            CreateMap<Contact, CreateContactDto>().ReverseMap();
            CreateMap<Contact, UpdateContactDto>().ReverseMap();
        }
    }
}
