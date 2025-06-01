using Microsoft.AspNetCore.Mvc;
using NewsManagementSystem.BLL.Services.Article;
using NewsManagementSystem.BLL.Services.Tag;

namespace NewsManagementSystem.Controllers
{
    public class TagController : Controller
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly ITagService _tagService;
        public TagController(ITagService tagService, ILogger<ArticleController> logger)
        {
            _logger = logger;
            _tagService = tagService;
        }
        public async Task<IActionResult> ListTag()
        {
            var role = HttpContext.Session.GetInt32("Role");
            if (role != 1)
                return RedirectToAction("AccessDenied", "Home");
            var tags = await _tagService.GetAllTagsAsync();
            return View(tags);
        }
    }
}
