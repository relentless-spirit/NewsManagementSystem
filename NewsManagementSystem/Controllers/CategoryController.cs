using Microsoft.AspNetCore.Mvc;
using NewsManagementSystem.BLL.Services.Category;

namespace NewsManagementSystem.Controllers;

public class CategoryController : Controller
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _logger = logger;
        _categoryService = categoryService;
    }
    // GET
    public async Task<IActionResult> ListCategories()
    {
        var categories = await _categoryService.GetCategoriesAsync();
        return View(categories);
    }
}