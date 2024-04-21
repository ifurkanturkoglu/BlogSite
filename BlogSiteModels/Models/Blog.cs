using System;
using System.Collections.Generic;

namespace BlogSiteModels.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string BlogTitle { get; set; } = null!;

    public string BlogDescription { get; set; } = null!;

    public string BlogText { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;
}
