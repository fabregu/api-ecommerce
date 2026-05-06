using SL_Api_Ecommerce.Models;
using SL_Api_Ecommerce.Models.Dtos;

namespace SL_Api_Ecommerce.Repository.IRepository
{
    public interface IUserRepository
    {
        ICollection<User>GetUsers();
        User? GetUser(int id);
        bool IsUniqueUser(string username);
        Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
        Task<User> Register(CreateUserDto createUserDto);
    }
}