using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // We want to Map AppUser properties to our defined MemberDto
            CreateMap<AppUser, MemberDto>()
                // here we configure where our 'PhotoUrl' sud be taken from
                .ForMember(destination => destination.PhotoUrl,
                opt => opt.MapFrom(source => source.Photos.FirstOrDefault(photo => photo.IsMain).Url))
                // here we directly specify how the value for the Age property sud be handled instead of using the GetAge() method we have inside AppUser entity
                .ForMember(destination => destination.Age,
                    opt => opt.MapFrom(source => source.DateOfBirth.CalculateAge()));
            // We want to Map Photo properties to our defined PhotoDto
            CreateMap<Photo, PhotoDto>();
        }
    }
}
