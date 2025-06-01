using System.ComponentModel.DataAnnotations;
using BusinessObject.Entities;

namespace NewsManagementSystem.Models;

public class ArticleViewModel
{
    public string NewsTitle { get; set; }
    public string Headline { get; set; }
    public string NewsContent { get; set; }
    public string NewsSource { get; set; }
    public string? NewsArticleID { get; set; }
    public short CategoryID { get; set; }

    public List<Tag> AllTags { get; set; } = new();
    public List<int> SelectedTagIds { get; set; } = new();
}

public class ArticleViewModelOrderByDescending : ArticleViewModel
{
    public DateTime? CreatedDate { get; set; }
    public bool? NewsStatus { get; set; }
    public short? CreatedByID { get; set; }
    public short? UpdatedByID { get; set; }
    public DateTime? ModifiedDate { get; set; }
}