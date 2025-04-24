using System;
using System.ComponentModel.DataAnnotations;

namespace Aesthetica.Models
{
    public class Room
    {
        public int RoomId { get; set; }          
        [Required]
        public string RoomName { get; set; }    
        [Required]
        public string Project { get; set; }      
        [Required]
        public string Status { get; set; }       
        [Required]
        public string Designer { get; set; }    
        [Required]
        public DateTime DueDate { get; set; }    
    }
}
