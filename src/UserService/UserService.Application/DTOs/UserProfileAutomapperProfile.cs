using AutoMapper;

namespace UserService.Application.DTOs;

public class UserProfileAutomapperProfile : Profile
{
    public UserProfileAutomapperProfile()
    {
        CreateMap<CreateUserProfileRequest, Domain.Entities.UserProfile>();

        CreateMap<UpdateUserProfileRequest, Domain.Entities.UserProfile>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcValue) => srcValue != null));

        CreateMap<Domain.Entities.UserProfile, UserProfileDto>();
    }
}