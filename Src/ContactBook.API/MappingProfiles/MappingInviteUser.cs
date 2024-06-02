using AutoMapper;
using ContactBook.Core.Dtos;
using ContactBook.Core.Entities;
using Profilee = AutoMapper.Profile;

namespace ContactBook.API.MappingProfiles
{
    public class MappingInviteUser : Profilee
    {
        public MappingInviteUser()
        {
            CreateMap<InviteUser, InviteUserDto>().ReverseMap();
            CreateMap<InviteUser, CreateInviteUserDto>().ReverseMap();
            CreateMap<InviteUser, UpdateInviteUserDto>().ReverseMap();
        }
    }
}
