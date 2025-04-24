using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Aesthetica.Models
{
    public class PropertyModel
    {
        [Key]  // ✅ This marks it as primary key
        public int PropertyId { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        
    }
}
