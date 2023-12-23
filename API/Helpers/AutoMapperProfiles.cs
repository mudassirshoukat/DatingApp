using API.DTO.AuthDtos;
using API.DTO.LikeDtos;
using API.DTO.MemberDtos;
using API.DTO.MessageDtos;
using API.DTO.PhotoDtos;
using API.Entities;
using API.Extentions;
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
            CreateMap<MemberUpdateRequestDto, AppUser>();
            CreateMap<RegisterRequestDto, AppUser>();

            CreateMap<AppUser, LikeResponseDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
           .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
           .ForMember(dest => dest.KnownAs, opt => opt.MapFrom(src => src.KnownAs))
           .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
           .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));

            CreateMap<Message, MessageDto>()
                .ForMember(x => x.SenderPhotourl, o => o.MapFrom(src => src.Sender.Photos.FirstOrDefault(s => s.IsMain).Url))
                .ForMember(x=>x.RecipientPhotoUrl,o=>o.MapFrom(src=>src.Recipient.Photos.FirstOrDefault(s=>s.IsMain).Url));
       
        }
    }
}
