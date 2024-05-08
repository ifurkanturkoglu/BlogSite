using BlogSiteModels.Models;

namespace BlogSite.Models
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public string CommentWriter { get; set; }
        public string CommentText { get; set; }
        public string CommentAddTime { get; set; }
        public int? ParentCommentId { get; set; }
        public ICollection<Comment>? CommentAnswers { get; set; }

    }
}
