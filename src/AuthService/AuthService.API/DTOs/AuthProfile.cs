using AuthService.API.Models;
using AutoMapper;

namespace AuthService.API.DTOs;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<UserProfileResponse, AuthUserResponse>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        CreateMap<User, AuthUserResponse>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
    }
}