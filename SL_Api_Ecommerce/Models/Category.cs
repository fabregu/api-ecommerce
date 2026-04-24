using System.ComponentModel.DataAnnotations;

namespace SL_Api_Ecommerce.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
    }
}