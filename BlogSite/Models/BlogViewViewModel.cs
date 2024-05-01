namespace BlogSite.Models
{
    public class BlogViewViewModel
    {
        public int BlogId { get; set; }

        public string BlogTitle { get; set; } = null!;

        public string BlogDescription { get; set; } = null!;

        public string BlogText { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
        public string? BlogWriter { get; set; }
    }
}
