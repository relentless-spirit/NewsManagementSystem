﻿using BusinessObject.Entities;

namespace NewsManagementSystem.DAL.Repositories.Article;

public interface IArticleRepo
{
    Task<List<NewsArticle>> GetArticlesync();
    Task<NewsArticle?> GetArticleByNameAsync(string name);
    Task<List<NewsArticle>> GetArticlesByCategoryIdAsync(short categoryId);
    Task CreateArticleAsync(NewsArticle category);
    Task UpdateArticleAsync(NewsArticle category, List<int> selectedTagIds);
    Task DeleteArticleAsync(NewsArticle category);
    
}