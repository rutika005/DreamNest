using System.ComponentModel.DataAnnotations;

namespace Aesthetica.Models
{
    public class LoginModel
    {
        [Key] // Primary Key for database
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Pass { get; set; } // Renamed for clarity

    }
}
