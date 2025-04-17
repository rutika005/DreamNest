namespace Aesthetica.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string AuthorImage { get; set; } 
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string Thumbnail { get; set; } 
    }

}
