using AutoMapper;
using UserService.Domain.Entities;

namespace UserService.Application.DTOs;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(x => x.Role));

        CreateMap<CreateUserRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        CreateMap<UpdateUserRequest, User>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcValue) => srcValue != null));
    }
}