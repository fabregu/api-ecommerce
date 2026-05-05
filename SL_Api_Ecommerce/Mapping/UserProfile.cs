using AutoMapper;
using SL_Api_Ecommerce.Models;
using SL_Api_Ecommerce.Models.Dtos;

namespace SL_Api_Ecommerce.Mapping
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<User, UserLoginResponseDto>().ReverseMap();
        }
    }
}