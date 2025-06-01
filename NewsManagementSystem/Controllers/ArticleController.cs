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

    //anyone
    public async Task<IActionResult> GetArticlesync(string? search)
    {
        List<NewsArticle> articles;
        if (search != null)
            articles = await _articleService.GetArticleByNameAsync(search);
        else
            articles = await _articleService.GetArticlesync();
        ViewBag.Search = search;
        return View("GetActiveArticle", articles);
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

    //staff
    [HttpGet]
    public async Task<IActionResult> CreateNewArticle(short categoryId)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return RedirectToAction("AccessDenied", "Home");

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
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return Unauthorized();

        if (!ModelState.IsValid)
        {
            vm.AllTags = await _tagService.GetAllTagsAsync();
            return View(vm);
        }
        var userId = HttpContext.Session.GetInt32("UserID");
        var article = new NewsArticle
        {
            NewsArticleID = Guid.NewGuid().ToString("N").Substring(0, 20),
            NewsTitle = vm.NewsTitle,
            Headline = vm.Headline,
            NewsContent = vm.NewsContent,
            NewsSource = vm.NewsSource,
            CreatedDate = DateTime.Now,
            CategoryID = vm.CategoryID,
            CreatedByID = userId.HasValue ? (short?)userId.Value : null,
            NewsStatus = true

        };

        await _articleService.CreateArticleWithTagsAsync(article, vm.SelectedTagIds);
        return RedirectToAction("ListArticles", new { categoryId = vm.CategoryID });
    }

    //staff
    public async Task<IActionResult> Update(string id)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return RedirectToAction("AccessDenied", "Home");

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
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return Unauthorized();

        if (id != vm.NewsArticleID) return NotFound();

        if (!ModelState.IsValid)
        {
            vm.AllTags = await _tagService.GetAllTagsAsync();
            return View(vm);
        }
        var userId = HttpContext.Session.GetInt32("UserID");
        var article = new NewsArticle
        {
            NewsArticleID = vm.NewsArticleID!,
            NewsTitle = vm.NewsTitle,
            Headline = vm.Headline,
            NewsContent = vm.NewsContent,
            NewsSource = vm.NewsSource,
            ModifiedDate = DateTime.Now,
            CategoryID = vm.CategoryID,
        };

        await _articleService.UpdateArticleWithTagsAsync(article, vm.SelectedTagIds);
        return RedirectToAction("ListArticles", new { categoryId = vm.CategoryID });
    }

    //staff
    public async Task<IActionResult> Delete(string id)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return RedirectToAction("AccessDenied", "Home");

        var article = await _articleService.GetArticleByIdWithTagsAsync(id);
        if (article == null) return NotFound();
        return View(article);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return Unauthorized();

        var article = await _articleService.GetArticleByIdWithTagsAsync(id);
        await _articleService.DeleteArticleByIdAsync(id);
        return RedirectToAction("ListArticles", new { categoryId = article.CategoryID });
    }

    [HttpGet]
    public async Task<IActionResult> GetArticlesByRangeDate(DateTime? startDate, DateTime? endDate)
    {
            var role = HttpContext.Session.GetInt32("Role");
            if (role != 0)
                return RedirectToAction("AccessDenied", "Home");
            var articles = await _articleService.GetArticleByDateRange(startDate, endDate);
            var view = articles.Select(a => new ArticleViewModel
            {
                NewsArticleID = a.NewsArticleID,
                NewsTitle = a.NewsTitle,
                Headline = a.Headline,
              //  NewsContent = a.NewsContent,
              //  NewsSource = a.NewsSource,
                CategoryID = a.Category?.CategoryID ?? 0,
                CreatedDate = a.CreatedDate,
                ModifiedDate = a.ModifiedDate,
                AllTags = a.Tags?.ToList() ?? new List<Tag>(),
            }).ToList();
        ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd") ?? "";
        ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd") ?? "";

        return View("Report", view);
    }
    
    public async Task<IActionResult> MyHistory()
    {
        var userId = HttpContext.Session.GetInt32("UserID");
        if (userId == null)
            return RedirectToAction("Login", "SystemAccount");

        var articles = await _articleService.GetArticlesByAccountIdAsync((short)userId.Value);
        var role = HttpContext.Session.GetInt32("Role");
        ViewBag.Role = role;
        return View("MyHistory", articles);
    }
    
}

