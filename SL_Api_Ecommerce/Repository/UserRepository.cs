using Microsoft.IdentityModel.Tokens;
using SL_Api_Ecommerce.Data;
using SL_Api_Ecommerce.Models;
using SL_Api_Ecommerce.Models.Dtos;
using SL_Api_Ecommerce.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SL_Api_Ecommerce.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly ApplicationDbContext _dbContext;
        private string secretKey;

        public UserRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
        }

        public User? GetUser(int id)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public ICollection<User> GetUsers()
        {
            return _dbContext.Users.OrderBy(u => u.Username).ToList();
        }

        public bool IsUniqueUser(string username)
        {
            return !_dbContext.Users.Any(u => u.Username.ToLower().Trim() == username.ToLower().Trim());
        }

        public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            if (string.IsNullOrEmpty(userLoginDto.Username))
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null,
                    Message = "El nombre de usuario es obligatorio"
                };
            }
            var user = _dbContext.Users.FirstOrDefault<User>(u => u.Username.ToLower().Trim() == userLoginDto.Username.ToLower().Trim());

            if (user == null)
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null,
                    Message = "Nombre de usuario no encontrado"
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password))
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null,
                    Message = "Credenciales incorrectas"
                };
            }

            var handlerToken = new JwtSecurityTokenHandler();

            if(string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException("Secret key is not configured.");
            }
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {   
                    new Claim("id", user.Id.ToString()),
                    new Claim("username", user.Username),
                    new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = handlerToken.CreateToken(tokenDescriptor);

            return new UserLoginResponseDto()
            {
                Token = handlerToken.WriteToken(token),
                User = new UserRegisterDto
                {
                    Username = user.Username,
                    Name = user.Name,
                    Role = user.Role,
                    Password = user.Password ?? ""
                },
                Message = "Inicio de sesión exitoso"
            };
        }


        public async Task<User> Register(CreateUserDto createUserDto)
        {
            var encryptedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password!);
            var user = new User
            {
                Name = createUserDto.Name,
                Username = createUserDto.Username ?? "No Username",
                Password = encryptedPassword,
                Role = createUserDto.Role
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}