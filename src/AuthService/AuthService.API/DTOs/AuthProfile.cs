using AutoMapper;

namespace AuthService.API.DTOs;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<UserServiceUserDto, AuthUserDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        CreateMap<UserServiceUserDto, LoginResponseDto>()
            .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
            .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));
    }
}