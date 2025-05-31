using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsManagementSystem.BLL.Services.Article;
using NewsManagementSystem.BLL.Services.Tag;
using NewsManagementSystem.Models;

namespace NewsManagementSystem.Controllers;

public class ArticleController : Controller
{
    private readonly ILogger<ArticleController> _logger;
    private readonly IArticleService _articleService;
    private readonly ITagService _tagService;

    public ArticleController(IArticleService articleService, ILogger<ArticleController> logger, ITagService tagService)
    {
        _logger = logger;
        _articleService = articleService;
        _tagService = tagService;
    }
 
    public async Task<IActionResult> ListArticles()
    {
        var articles = await _articleService.GetArticlesync();
        return View(articles);
    }

  
    public async Task<IActionResult> Create()
    {
        var vm = new ArticleViewModel
        {
            AllTags = await _tagService.GetAllTagsAsync()
        };
        return View(vm);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ArticleViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.AllTags = await _tagService.GetAllTagsAsync(); // reload tag list if failed
            return View(vm);
        }

        var article = new NewsArticle
        {
            NewsArticleID = Guid.NewGuid().ToString(),
            NewsTitle = vm.NewsTitle,
            Headline = vm.Headline,
            NewsContent = vm.NewsContent,
            NewsSource = vm.NewsSource,
            CreatedDate = DateTime.Now
        };

        await _articleService.CreateArticleWithTagsAsync(article, vm.SelectedTagIds);
        return RedirectToAction("ListArticles");
    }

    
    public async Task<IActionResult> Update(string id)
    {
        var article = await _articleService.GetArticleByIdWithTagsAsync(id);
        if (article == null) return NotFound();

        var vm = new ArticleViewModel
        {
            NewsArticleID = article.NewsArticleID,
            NewsTitle = article.NewsTitle,
            Headline = article.Headline,
            NewsContent = article.NewsContent,
            NewsSource = article.NewsSource,
            SelectedTagIds = article.Tags.Select(t => t.TagID).ToList(),
            AllTags = await _tagService.GetAllTagsAsync()
        };

        return View(vm);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(string id, ArticleViewModel vm)
    {
        if (id != vm.NewsArticleID) return NotFound();

        if (!ModelState.IsValid)
        {
            vm.AllTags = await _tagService.GetAllTagsAsync();
            return View(vm);
        }

        var article = new NewsArticle
        {
            NewsArticleID = vm.NewsArticleID!,
            NewsTitle = vm.NewsTitle,
            Headline = vm.Headline,
            NewsContent = vm.NewsContent,
            NewsSource = vm.NewsSource,
            ModifiedDate = DateTime.Now
        };

        await _articleService.UpdateArticleWithTagsAsync(article, vm.SelectedTagIds);
        return RedirectToAction(nameof(ListArticles));
    }

    
    public async Task<IActionResult> Delete(string id)
    {
        var article = await _articleService.GetArticleByIdWithTagsAsync(id);
        if (article == null) return NotFound();
        return View(article);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        await _articleService.DeleteArticleByIdAsync(id);
        return RedirectToAction(nameof(ListArticles));
    }
    
    
}