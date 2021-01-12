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
            // AppUser (represent the Users table in d database) is on the left so it is the source (it contains the data we need)
            // Each property in the MemberDto takes its value from d field dt matches their name in AppUser model... So 'UserName' field in MemberDto will take the value of 'UserName' in AppUser (and UserName in AppUser takes d value of UserName column in the database for the current user)
            CreateMap<AppUser, MemberDto>()

                // here we configure where our 'PhotoUrl' sud be taken from
                .ForMember(destination => destination.PhotoUrl,
                opt => opt.MapFrom(source => source.Photos.FirstOrDefault(photo => photo.IsMain).Url))

                // here we directly specify how the value for the Age property sud be handled instead of using the GetAge() method we have inside AppUser entity
                .ForMember(destination => destination.Age,
                    opt => opt.MapFrom(source => source.DateOfBirth.CalculateAge()));

            // Every field inside PhotoDto will map to a corresponding field inside Photo model and take d values of those fields inside Photo model
            CreateMap<Photo, PhotoDto>();

            //Since we are updating a user with MemberUpdateDto, MemberUpdateDto will be the source and will be on the left because we want to replace all the values of the fields inside AppUser that matches a field inside MemberUpdateDto to the values we are sending from the frontend....
            CreateMap<MemberUpdateDto, AppUser>();

            //We register a user with RegisterDto and then transform it to an AppUser so AppUser takes all the values RegisterDto is sending
            CreateMap<RegisterDto, AppUser>();
        }
    }
}
