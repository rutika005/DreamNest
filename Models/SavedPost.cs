namespace Aesthetica.Models
{
    public class SavedPost
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string Category { get; set; }
        public DateTime? SavedAt { get; set; } 

    }
}
