using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSiteModels.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; } = null!;
        public DateTime CommentAddTime { get; set; }
        public ICollection<Comment>? CommentAnswers { get; set; }
        public int? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
