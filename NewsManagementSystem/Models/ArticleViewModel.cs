using System.ComponentModel.DataAnnotations;
using BusinessObject.Entities;

namespace NewsManagementSystem.Models;

public class ArticleViewModel
{
    public string? NewsArticleID { get; set; }

    [Required]
    [StringLength(400)]
    public string? NewsTitle { get; set; }

    [Required]
    [StringLength(150)]
    public string? Headline { get; set; }

    [StringLength(4000)]
    public string? NewsContent { get; set; }

    [StringLength(400)]
    public string? NewsSource { get; set; }

    [Display(Name = "Tags")]
    public List<int> SelectedTagIds { get; set; } = new();

    public List<Tag> AllTags { get; set; } = new();
}