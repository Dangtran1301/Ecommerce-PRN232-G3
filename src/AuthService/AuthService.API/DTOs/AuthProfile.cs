using AutoMapper;

namespace AuthService.API.DTOs;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<UserServiceUserDto, AuthUserDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
    }
}