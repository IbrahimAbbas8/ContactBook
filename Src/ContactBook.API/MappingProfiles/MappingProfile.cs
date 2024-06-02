using AutoMapper;
using ContactBook.Core.Dtos;
using ProfileMap = AutoMapper.Profile;
using Profile = ContactBook.Core.Entities.Profile;

namespace ContactBook.API.MappingProfiles
{
    public class MappingProfile : ProfileMap
    {
        public MappingProfile()
        {
            CreateMap<Profile, ProfileDto>().ReverseMap();
        }
    }
}
