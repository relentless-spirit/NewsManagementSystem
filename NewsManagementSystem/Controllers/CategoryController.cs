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
    public IActionResult Create()
    {
        return View();
    }
    // GET
    public async Task<IActionResult> ListCategories()
    {
        var categories = await _categoryService.GetCategoriesAsync();
        return View(categories);
    }
    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        // Check if the category name already exists
        var existingCategory = await _categoryService.GetCategoryByNameAsync(category.CategoryName);
        if (existingCategory != null)
        {
            ModelState.AddModelError("CategoryName", "A category with this name already exists.");
            return View(category);
        }


        await _categoryService.CreateCategoryAsync(category);
        
        return RedirectToAction(nameof(ListCategories));
    }
    public async Task<IActionResult> Edit(short id)
    {
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
    [HttpPost]
    public async Task<IActionResult> Edit(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        // Check if the category exists
        var existingCategory = await _categoryService.GetCategoryByIdAsync(category.CategoryID);
        if (existingCategory == null)
        {
            return NotFound("Category not found.");
        }

        // Check for duplicate category name (excluding the current category)
        var duplicateCategory = await _categoryService.GetCategoryByNameAsync(category.CategoryName);
        if (duplicateCategory != null && duplicateCategory.CategoryID != category.CategoryID)
        {
            ModelState.AddModelError("CategoryName", "A category with this name already exists.");
            return View(category);
        }

        await _categoryService.UpdateCategoryAsync(category);
        return RedirectToAction(nameof(ListCategories));
    }
    public async Task<IActionResult> Delete(short id)
    {
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
        if (id <= 0)
        {
            return BadRequest("Invalid category ID.");
        }

        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound("Category not found.");
        }

        // Check if the category exists in the NewsArticle table
        var isCategoryInUse = await _categoryService.CategoryExistsAsync(id);
        if (isCategoryInUse)
        {
            ModelState.AddModelError(string.Empty, "This category cannot be deleted because it is associated with one or more news articles.");
            return View("Delete", category); // Return the Delete view with the error message
        }

        await _categoryService.DeleteCategoryAsync(category);
        return RedirectToAction(nameof(ListCategories));
    }


}