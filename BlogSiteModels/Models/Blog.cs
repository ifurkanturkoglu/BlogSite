using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace BlogSiteModels.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string BlogTitle { get; set; } = null!;

    public string BlogDescription { get; set; } = null!;

    public string BlogText { get; set; } = null!;

    public DateTime BlogAddDate { get; set; }
    public string ImageUrl { get; set; }
    public int UserID { get; set; }
    public User User { get; set; }
}
