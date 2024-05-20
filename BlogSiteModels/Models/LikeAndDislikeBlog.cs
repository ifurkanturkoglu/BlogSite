using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSiteModels.Models
{
    public class LikeAndDislikeBlog
    {
        public int LikeAndDislikeBlogId { get; set; }
        public int BlogId { get; set; }
        public string UserName { get; set; }
        public bool IsLiked { get; set; }
        public Blog Blog { get; set; }
    }
}
