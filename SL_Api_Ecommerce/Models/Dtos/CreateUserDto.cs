using System.ComponentModel.DataAnnotations;

namespace SL_Api_Ecommerce.Models.Dtos
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "El rol es requerido")]
        public string? Role { get; set; }
    }
}