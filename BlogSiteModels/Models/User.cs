using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSiteModels.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
        public ICollection<Blog>? UserBlogs { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }

    public enum UserType
    {
        Admin, User, Guest
    }
}

