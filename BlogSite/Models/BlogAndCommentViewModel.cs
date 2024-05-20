using BlogSiteModels.Models;

namespace BlogSite.Models
{
    public class BlogAndCommentViewModel
    {
        public int BlogId { get; set; }

        public string BlogTitle { get; set; } = null!;

        public string BlogDescription { get; set; } = null!;

        public string BlogText { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
        public int BlogLikeCount { get; set; }
        public int BlogDislikeCount { get; set; }
        public bool? IsLiked { get; set; }
        public string? BlogWriter { get; set; }
        public ICollection<CommentViewModel>? Comments { get; set; }
    }
}
