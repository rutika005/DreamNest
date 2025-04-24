using System;
using System.ComponentModel.DataAnnotations;

namespace Aesthetica.Models
{
    public class Room
    {
        public int RoomId { get; set; }           // Unique ID for the room
        [Required]
        public string RoomName { get; set; }      // Name of the room
        [Required]
        public string Project { get; set; }       // Project associated with the room
        [Required]
        public string Status { get; set; }        // Status of the room (e.g., In Progress, Completed, On Hold)
        [Required]
        public string Designer { get; set; }      // Designer handling the room
        [Required]
        public DateTime DueDate { get; set; }     // Due date for the project
    }
}
