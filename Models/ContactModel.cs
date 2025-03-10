using System.ComponentModel.DataAnnotations;

namespace Aesthetica.Models
{
    public class ContactModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message can't exceed 500 characters")]
        public string Message { get; set; }
    }
}
