using System.ComponentModel.DataAnnotations;

namespace Aesthetica.Models
{
    public class ForgotpassModel
    {
        [Key] 
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}
