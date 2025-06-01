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
 
    public async Task<IActionResult> ListArticles(short categoryId)
    {
        var articles = await _articleService.GetArticlesByCategoryIdAsync(categoryId);
        ViewData["CategoryId"] = categoryId;
        return View(articles);
    }


    [HttpGet]
    public async Task<IActionResult> GetArticlesByCategory(short categoryId)
    {
        var articles = await _articleService.GetArticlesByCategoryIdAsync(categoryId);
        return Json(articles);
    }


    public async Task<IActionResult> GetActiveArticle()
    {
        var articles = await _articleService.GetActiveArticlesAsync();
        return View("GetActiveArticle", articles);
    }




    [HttpGet]
    public async Task<IActionResult> CreateNewArticle(short categoryId)
    {
        var vm = new ArticleViewModel
        {
            CategoryID = categoryId,
            AllTags = await _tagService.GetAllTagsAsync()
        };
        return View("CreateNewArticle", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateNewArticle(ArticleViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.AllTags = await _tagService.GetAllTagsAsync();
            return View(vm);
        }

        var article = new NewsArticle
        {
            NewsArticleID = Guid.NewGuid().ToString().Substring(0, 20),
            NewsTitle = vm.NewsTitle,
            Headline = vm.Headline,
            NewsContent = vm.NewsContent,
            NewsSource = vm.NewsSource,
            CreatedDate = DateTime.Now,
            CategoryID = vm.CategoryID
        };

        await _articleService.CreateArticleWithTagsAsync(article, vm.SelectedTagIds);
        return RedirectToAction("ListArticles", new { categoryId = vm.CategoryID });
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
            CategoryID = article.CategoryID ?? 0,
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
            ModifiedDate = DateTime.Now,
            CategoryID = vm.CategoryID
        };

        await _articleService.UpdateArticleWithTagsAsync(article, vm.SelectedTagIds);
        return RedirectToAction("ListArticles", new { categoryId = vm.CategoryID });
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
        var article = await _articleService.GetArticleByIdWithTagsAsync(id); 

        await _articleService.DeleteArticleByIdAsync(id);
        return RedirectToAction("ListArticles", new { categoryId = article.CategoryID });
    }
    
    
}