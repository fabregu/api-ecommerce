using System.ComponentModel.DataAnnotations;

namespace SL_Api_Ecommerce.Models.Dtos
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string? Password { get; set; }
    }
}