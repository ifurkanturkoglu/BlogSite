namespace BlogSite.Models
{
    public class BlogAddViewModel
    {
        public int BlogId { get; set; }

        public string BlogTitle { get; set; } = null!;

        public string BlogDescription { get; set; } = null!;

        public string BlogText { get; set; } = null!;

        public IFormFile Image { get; set; } = null!;
        public string? BlogWriter { get; set; }
    }
}
