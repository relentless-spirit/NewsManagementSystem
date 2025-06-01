using BusinessObject.Entities;
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

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return RedirectToAction("AccessDenied", "Home");

        return PartialView();
    }

    public async Task<IActionResult> ListCategories()
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return RedirectToAction("AccessDenied", "Home");

        var categories = await _categoryService.GetCategoriesAsync();
        return View(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return Unauthorized();

        if (!ModelState.IsValid)
            return PartialView(category);

        var existing = await _categoryService.GetCategoryByNameAsync(category.CategoryName);
        if (existing != null)
        {
            ModelState.AddModelError("CategoryName", "A category with this name already exists.");
            return PartialView(category);
        }

        await _categoryService.CreateCategoryAsync(category);
        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(short id)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return RedirectToAction("AccessDenied", "Home");

        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();

        return PartialView(category);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Category category)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return Unauthorized();

        if (!ModelState.IsValid)
            return PartialView(category);

        var duplicate = await _categoryService.GetCategoryByNameAsync(category.CategoryName);
        if (duplicate != null && duplicate.CategoryID != category.CategoryID)
        {
            ModelState.AddModelError("CategoryName", "A category with this name already exists.");
            return PartialView(category);
        }

        await _categoryService.UpdateCategoryAsync(category);
        return Json(new { success = true });
    }

    public async Task<IActionResult> Delete(short id)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return RedirectToAction("AccessDenied", "Home");

        if (id <= 0)
        {
            return BadRequest("Invalid category ID.");
        }

        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound("Category not found.");
        }

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeletePost(short id)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
            return Unauthorized();

        if (id <= 0)
        {
            return BadRequest("Invalid category ID.");
        }

        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound("Category not found.");
        }

        var isCategoryInUse = await _categoryService.CategoryExistsAsync(id);
        if (isCategoryInUse)
        {
            ModelState.AddModelError(string.Empty, "This category cannot be deleted because it is associated with one or more news articles.");
            return View("Delete", category);
        }

        await _categoryService.DeleteCategoryAsync(category);
        return RedirectToAction(nameof(ListCategories));
    }
}
