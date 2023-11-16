using API.DTO.MemberDtos;
using API.DTO.PhotoDtos;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberResponseDto>()
            .ForMember(dist => dist.PhotoUrl,
            opt => opt.MapFrom(src => src.Photos.FirstOrDefault(a => a.IsMain).Url));
            
            CreateMap<Photo, PhotoResponseDto>();
        }
    }
}
